using DPK.SCF.Grafana.Models.Events;

namespace DPK.SCF.Grafana.Contracts.Events
{
    /// <summary>
    /// قرارداد انتشار رویدادهای مرتبط با سلامت سرویس‌ها.
    /// </summary>
    public interface IHealthEventPublisher
    {
        Task PublishHealthCheckedAsync(HealthCheckedEvent @event, CancellationToken cancellationToken = default);

        Task PublishHealthMetricsChangedAsync(HealthMetricsChangedEvent @event, CancellationToken cancellationToken = default);
    }
}
