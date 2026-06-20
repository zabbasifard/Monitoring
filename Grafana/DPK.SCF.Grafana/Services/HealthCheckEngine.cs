using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using DPK.SCF.Grafana.Models.Events;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace DPK.SCF.Grafana.Services
{
    /// <summary>
    /// مسئول بررسی سلامت سرویس و وابستگی‌های آن و تولید نتیجه Health Check است.
    /// </summary>
    public class HealthCheckEngine : IHealthCheckEngine
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IMetricsManager _metricsManager;
        private readonly IHealthEventPublisher _eventPublisher;
        private readonly MonitoringOptions _monitoringOptions;

        public HealthCheckEngine(
            HealthCheckService healthCheckService,
            IMetricsManager metricsManager,
            IHealthEventPublisher eventPublisher,
            IOptions<MonitoringOptions> monitoringOptions)
        {
            _healthCheckService = healthCheckService;
            _metricsManager = metricsManager;
            _eventPublisher = eventPublisher;
            _monitoringOptions = monitoringOptions.Value;
        }

        public async Task<ServiceHealthDto> CheckAsync(CancellationToken cancellationToken = default)
        {
            var start = DateTime.UtcNow;

            var report = await _healthCheckService.CheckHealthAsync(
                predicate: check => check.Tags.Contains("ready") || check.Tags.Contains("live"),
                cancellationToken: cancellationToken);

            var components = report.Entries
                .Select(x => new ComponentHealthDto
                {
                    Name = x.Key,
                    Status = MapStatus(x.Value.Status),
                    Message = x.Value.Description ?? x.Value.Exception?.Message
                })
                .ToList();

            if (!components.Any())
            {
                components.Add(new ComponentHealthDto
                {
                    Name = "Application",
                    Status = ServiceHealthStatus.Up,
                    Message = "Application is running"
                });
            }

            var status = MapStatus(report.Status);

            var result = new ServiceHealthDto
            {
                ServiceId = _monitoringOptions.ServiceName,
                Status = status,
                CheckedAt = DateTime.UtcNow,
                LatencyMs = (DateTime.UtcNow - start).TotalMilliseconds,
                Components = components
            };

            _metricsManager.SetGauge(
                MetricDefinitions.HealthCheckLatency,
                result.LatencyMs,
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = result.ServiceId
                });

            _metricsManager.SetGauge(
                MetricDefinitions.HealthStatus,
                ConvertStatusToMetricValue(status),
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = result.ServiceId,
                    [MetricLabels.Status] = status
                });

            await _eventPublisher.PublishHealthCheckedAsync(
                new HealthCheckedEvent
                {
                    ServiceId = result.ServiceId,
                    Status = result.Status,
                    CheckedAt = result.CheckedAt
                },
                cancellationToken);

            return result;
        }

        private static string MapStatus(HealthStatus status)
        {
            return status switch
            {
                HealthStatus.Healthy => ServiceHealthStatus.Up,
                HealthStatus.Degraded => ServiceHealthStatus.Degraded,
                HealthStatus.Unhealthy => ServiceHealthStatus.Down,
                _ => ServiceHealthStatus.Down
            };
        }

        private static double ConvertStatusToMetricValue(string status)
        {
            return status switch
            {
                ServiceHealthStatus.Up => 1,
                ServiceHealthStatus.Degraded => 0.5,
                ServiceHealthStatus.Down => 0,
                _ => 0
            };
        }
    }
}