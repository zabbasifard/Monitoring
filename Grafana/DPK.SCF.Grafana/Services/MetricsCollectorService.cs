using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using DPK.SCF.Grafana.Models.Events;

namespace DPK.SCF.Grafana.Services
{
    /// <summary>
    /// مسئول دریافت، اعتبارسنجی و ثبت متریک‌های ارسالی از سرویس‌های مختلف.
    /// </summary>
    public class MetricsCollectorService : IMetricsCollectorService
    {
        private readonly IMetricsManager _metricsManager;
        private readonly IMetricsEventPublisher _eventPublisher;

        public MetricsCollectorService(
            IMetricsManager metricsManager,
            IMetricsEventPublisher eventPublisher)
        {
            _metricsManager = metricsManager;
            _eventPublisher = eventPublisher;
        }

        public async Task CollectAsync(CreateMetricRequest request, CancellationToken cancellationToken = default)
        {
            var metric = ResolveMetric(request.MetricType);

            _metricsManager.SetGauge(
                metric,
                request.Value,
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = request.ServiceId
                });

            await _eventPublisher.PublishMetricsCollectedAsync(
                new MetricsCollectedEvent
                {
                    ServiceId = request.ServiceId,
                    MetricType = request.MetricType,
                    Value = request.Value,
                    Timestamp = DateTime.UtcNow
                },
                cancellationToken);

            await _eventPublisher.PublishMetricsPublishedAsync(
                new MetricsPublishedEvent
                {
                    ServiceName = request.ServiceId,
                    MetricName = request.MetricType,
                    Value = request.Value,
                    PublishedAt = DateTime.UtcNow
                },
                cancellationToken);

        }

        private static MetricDefinition ResolveMetric(string metricType)
        {
            return metricType switch
            {
                AppMetrics.Throughput => MetricDefinitions.Throughput,
                AppMetrics.ErrorRate => MetricDefinitions.ErrorRate,
                AppMetrics.ActiveUsers => MetricDefinitions.ActiveUsers,
                AppMetrics.QueueCount => MetricDefinitions.QueueCount,

                _ => throw new InvalidOperationException(
                    $"Metric type '{metricType}' is not supported.")
            };
        }
    }
}
