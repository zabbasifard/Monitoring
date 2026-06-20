namespace DPK.SCF.Grafana.Models.Constaints
{
    /// <summary>
    /// وضعیت‌های استاندارد سلامت سرویس و وابستگی‌های آن را تعریف می‌کند.
    /// </summary>
    public static class ServiceHealthStatus
    {
        public const string Up = "UP";
        public const string Down = "DOWN";
        public const string Degraded = "DEGRADED";
    }
}
