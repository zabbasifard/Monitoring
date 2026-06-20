namespace DPK.SCF.Grafana.Models.Constaints
{
    /// <summary>
    /// نام متریک‌های عمومی سامانه را نگهداری می‌کند.
    /// </summary>
    public static class AppMetrics
    {
        public const string Request = "request";
        public const string Duration = "duration";
        public const string ActiveUsers = "active_users";
        public const string QueueCount = "queue_count";
        public const string Latency = "latency";
        public const string Throughput = "throughput";
        public const string ErrorRate = "error_rate";
        public const string HealthStatus = "health_status";
        public const string HealthCheckLatency = "health_check_latency";
        public const string ReadinessStatus = "readiness_status";
        public const string CpuUsage = "cpu_usage";
        public const string MemoryUsage = "memory_usage";
    }
}

