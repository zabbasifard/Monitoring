namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// گزارش تجمیعی وضعیت سلامت سرویس‌های مانیتور شده را نگهداری می‌کند.
    /// </summary>
    public class HealthReportDto
    {
        public string OverallStatus { get; set; } = default!;
        public DateTime CheckedAt { get; set; }
        public List<ServiceHealthDto> Services { get; set; } = new();
    }
}
