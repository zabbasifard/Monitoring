using DPK.SCF.Grafana.Models;

namespace DPK.SCF.Grafana.Contracts
{
    /// <summary>
    /// قرارداد تجمیع و پردازش وضعیت سلامت سرویس‌های مختلف سامانه.
    /// </summary>
    public interface IHealthAggregatorService
    {
        Task<HealthReportDto> GetHealthAsync(string? serviceId, CancellationToken cancellationToken = default);
    }
}
