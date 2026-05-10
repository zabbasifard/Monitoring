using Prometheus.Contracts;

namespace Prometheus.Services
{
    public class MetricRegistery : IMetricsRegistery
    {
        public Counter CreateCounter(string name, string help, string description)
        => Metrics.CreateCounter(name, help, description);

        public Gauge CreateGauge(string name, string help, string description)
        => Metrics.CreateGauge(name, help, description);

        public Histogram CreateHistogram(string name, string help, string description)
        => Metrics.CreateHistogram(name, help, description);
    }
}
