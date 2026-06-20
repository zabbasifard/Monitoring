namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// تنظیمات مربوط به سرویس‌های تحت مانیتورینگ را نگهداری می‌کند.
    /// </summary>
    public class HealthMonitoringOptions
    {
        public List<MonitoredServiceOptions> Services { get; set; } = new();
    }
}
