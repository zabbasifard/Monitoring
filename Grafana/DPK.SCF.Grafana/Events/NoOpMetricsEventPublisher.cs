using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models.Events;

namespace DPK.SCF.Grafana.Events
{
    /// <summary>
    /// پیاده‌سازی پیش‌فرض و بدون عملیات برای انتشار رویدادهای متریک در حالت پکیج.
    /// </summary>
    public class NoOpMetricsEventPublisher : IMetricsEventPublisher
    {
        public Task PublishMetricsCollectedAsync(MetricsCollectedEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task PublishMetricsPublishedAsync(MetricsPublishedEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
