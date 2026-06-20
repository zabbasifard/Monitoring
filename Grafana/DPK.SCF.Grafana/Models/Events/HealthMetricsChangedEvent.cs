namespace DPK.SCF.Grafana.Models.Events
{
    /// <summary>
    /// رویداد تغییر وضعیت سلامت سرویس جهت استفاده در سیستم‌های هشدار و مانیتورینگ.
    /// </summary>
    public class HealthMetricsChangedEvent
    {
        public string ServiceName { get; set; } = default!;
        public string CurrentStatus { get; set; } = default!;
        public DateTime ChangedAt { get; set; }
    }
}
