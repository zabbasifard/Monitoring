namespace Prometheus.Contracts
{
    public interface IMetricsRegistery
    {
        Counter CreateCounter(string name, string help, string description);
        Gauge CreateGauge(string name, string help, string description);
        Histogram CreateHistogram(string name, string help, string description);
    }
}
