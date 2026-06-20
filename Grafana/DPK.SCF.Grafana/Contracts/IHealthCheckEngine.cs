using DPK.SCF.Grafana.Models;

namespace DPK.SCF.Grafana.Contracts
{
    /// <summary>
    /// قرارداد بررسی و محاسبه وضعیت سلامت سرویس جاری.
    /// </summary>
    public interface IHealthCheckEngine
    {
        Task<ServiceHealthDto> CheckAsync(CancellationToken cancellationToken = default);
    }
}
