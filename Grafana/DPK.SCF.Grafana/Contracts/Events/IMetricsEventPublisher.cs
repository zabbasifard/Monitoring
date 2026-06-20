using DPK.SCF.Grafana.Models.Events;

namespace DPK.SCF.Grafana.Contracts.Events
{
    /// <summary>
    /// قرارداد انتشار رویدادهای مرتبط با متریک‌ها.
    /// </summary>
    public interface IMetricsEventPublisher
    {
        Task PublishMetricsCollectedAsync(MetricsCollectedEvent @event, CancellationToken cancellationToken = default);

        Task PublishMetricsPublishedAsync(MetricsPublishedEvent @event, CancellationToken cancellationToken = default);
    }

}
