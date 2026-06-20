using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using DPK.SCF.Grafana.Models.Events;
using Microsoft.Extensions.Options;

namespace DPK.SCF.Grafana.Services
{
    /// <summary>
    /// وضعیت سلامت سرویس‌های پیکربندی شده را جمع‌آوری و گزارش تجمیعی سلامت سامانه را تولید می‌کند.
    /// </summary>
    public class HealthAggregatorService : IHealthAggregatorService
    {
        private readonly HttpClient _httpClient;
        private readonly HealthMonitoringOptions _options;
        private readonly IMetricsManager _metricsManager;
        private readonly IHealthEventPublisher _eventPublisher;
        private readonly IHealthStatusStateStore _healthStatusStateStore;

        public HealthAggregatorService(
            HttpClient httpClient,
            IOptions<HealthMonitoringOptions> options,
            IMetricsManager metricsManager,
            IHealthEventPublisher eventPublisher,
            IHealthStatusStateStore healthStatusStateStore)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _metricsManager = metricsManager;
            _eventPublisher = eventPublisher;
            _healthStatusStateStore = healthStatusStateStore;
        }

        public async Task<HealthReportDto> GetHealthAsync(string? serviceId, CancellationToken cancellationToken = default)
        {
            var services = _options.Services;

            if (!string.IsNullOrWhiteSpace(serviceId))
            {
                services = services
                    .Where(x => x.ServiceName.Equals(serviceId, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var results = await Task.WhenAll(
                services.Select(x => CheckServiceAsync(x, cancellationToken)));

            return new HealthReportDto
            {
                CheckedAt = DateTime.UtcNow,
                Services = results.ToList(),
                OverallStatus = CalculateOverallStatus(results)
            };
        }

        private async Task<ServiceHealthDto> CheckServiceAsync(MonitoredServiceOptions service, CancellationToken cancellationToken)
        {
            var start = DateTime.UtcNow;

            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(service.TimeoutSeconds));

                var response = await _httpClient.GetAsync(service.HealthUrl, cts.Token);

                var latency = (DateTime.UtcNow - start).TotalMilliseconds;

                var status = response.IsSuccessStatusCode
                    ? ServiceHealthStatus.Up
                    : ServiceHealthStatus.Degraded;

                RegisterHealthMetric(service.ServiceName, status);

                if (_healthStatusStateStore.HasChanged(service.ServiceName, status))
                {
                    await _eventPublisher.PublishHealthMetricsChangedAsync(
                        new HealthMetricsChangedEvent
                        {
                            ServiceName = service.ServiceName,
                            CurrentStatus = status,
                            ChangedAt = DateTime.UtcNow
                        },
                        cancellationToken);
                }

                return new ServiceHealthDto
                {
                    ServiceId = service.ServiceName,
                    Status = status,
                    CheckedAt = DateTime.UtcNow,
                    LatencyMs = latency
                };
            }
            catch
            {
                RegisterHealthMetric(service.ServiceName, ServiceHealthStatus.Down);

                if (_healthStatusStateStore.HasChanged(
                        service.ServiceName,
                        ServiceHealthStatus.Down))
                {
                    await _eventPublisher.PublishHealthMetricsChangedAsync(
                        new HealthMetricsChangedEvent
                        {
                            ServiceName = service.ServiceName,
                            CurrentStatus = ServiceHealthStatus.Down,
                            ChangedAt = DateTime.UtcNow
                        },
                        cancellationToken);
                }

                return new ServiceHealthDto
                {
                    ServiceId = service.ServiceName,
                    Status = ServiceHealthStatus.Down,
                    CheckedAt = DateTime.UtcNow,
                    LatencyMs = 0
                };
            }
        }

        private void RegisterHealthMetric(string serviceName, string status)
        {
            _metricsManager.SetGauge(
                MetricDefinitions.HealthStatus,
                status == ServiceHealthStatus.Up ? 1 : 0,
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = serviceName,
                    [MetricLabels.Status] = status
                });
        }

        private static string CalculateOverallStatus(IEnumerable<ServiceHealthDto> services)
        {
            if (services.Any(x => x.Status == ServiceHealthStatus.Down))
                return ServiceHealthStatus.Down;

            if (services.Any(x => x.Status == ServiceHealthStatus.Degraded))
                return ServiceHealthStatus.Degraded;

            return ServiceHealthStatus.Up;
        }
    }
}
