namespace DPK.SCF.Grafana.Models.Constaints
{
    /// <summary>
    /// متریک‌های عمومی قابل استفاده در سرویس‌های مختلف سامانه.
    /// </summary>
    public static class MetricDefinitions
    {
        public static readonly MetricDefinition RequestTotal =
            new(
                AppMetrics.Request,
                MetricType.Counter,
                new[]
                {
                MetricLabels.Service,
                MetricLabels.Status
                });

        public static readonly MetricDefinition Duration =
            new(
                AppMetrics.Duration,
                MetricType.Histogram,
                new[]
                {
                MetricLabels.Service
                });

        public static readonly MetricDefinition HealthStatus =
            new(
                AppMetrics.HealthStatus,
                MetricType.Gauge,
                new[]
                {
                MetricLabels.Service,
                MetricLabels.Status
                });

        public static readonly MetricDefinition Throughput =
            new(
                AppMetrics.Throughput,
                MetricType.Gauge,
                new[]
                {
                MetricLabels.Service
                });

        public static readonly MetricDefinition ErrorRate =
            new(
                AppMetrics.ErrorRate,
                MetricType.Gauge,
                new[]
                {
                MetricLabels.Service
                });

        public static readonly MetricDefinition ActiveUsers =
            new(
                AppMetrics.ActiveUsers,
                MetricType.Gauge,
                new[]
                {
                    MetricLabels.Service
                });

        public static readonly MetricDefinition QueueCount =
          new(
              AppMetrics.QueueCount,
              MetricType.Gauge,
              new[]
              {
                    MetricLabels.Service
              });

        public static readonly MetricDefinition HealthCheckLatency =
           new(
               AppMetrics.HealthCheckLatency,
               MetricType.Gauge,
               new[]
               {
                   MetricLabels.Service
               });

        public static readonly MetricDefinition ReadinessStatus =
            new(
                AppMetrics.ReadinessStatus,
                MetricType.Gauge,
                new[]
                {
                    MetricLabels.Service
                });

        public static readonly MetricDefinition CpuUsage =
            new(
                AppMetrics.CpuUsage,
                MetricType.Gauge,
                new[] { MetricLabels.Service });

        public static readonly MetricDefinition MemoryUsage =
            new(
                AppMetrics.MemoryUsage,
                MetricType.Gauge,
                new[] { MetricLabels.Service });

    }
}
