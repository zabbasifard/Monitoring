namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// اطلاعات مورد نیاز برای مانیتورینگ و بررسی سلامت یک سرویس را نگهداری می‌کند.
    /// </summary>
    public class MonitoredServiceOptions
    {
        public string ServiceName { get; set; } = default!;
        public string HealthUrl { get; set; } = default!;
        public int TimeoutSeconds { get; set; } = 3;
    }
}
