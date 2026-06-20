namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// تنظیمات پایه مانیتورینگ سرویس را نگهداری می‌کند.
    /// </summary>
    public sealed class MonitoringOptions
    {
        public string ServiceName { get; set; } = default!;

        public string Environment { get; set; } = "default";
    }
}
