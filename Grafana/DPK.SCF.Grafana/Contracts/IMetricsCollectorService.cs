using DPK.SCF.Grafana.Models;

namespace DPK.SCF.Grafana.Contracts
{
    /// <summary>
    /// قرارداد دریافت و ثبت متریک‌های ارسالی از سرویس‌های مختلف.
    /// </summary>
    public interface IMetricsCollectorService
    {
        Task CollectAsync(CreateMetricRequest request, CancellationToken cancellationToken = default);
    }
}
