using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models.Events;

namespace DPK.SCF.Grafana.Events
{
    /// <summary>
    /// پیاده‌سازی پیش‌فرض و بدون عملیات برای انتشار رویدادهای سلامت در حالت پکیج.
    /// </summary>
    public class NoOpHealthEventPublisher : IHealthEventPublisher
    {
        public Task PublishHealthCheckedAsync(HealthCheckedEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task PublishHealthMetricsChangedAsync(HealthMetricsChangedEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
