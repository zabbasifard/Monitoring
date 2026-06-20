namespace DPK.SCF.Grafana.Models.Constaints
{
    /// <summary>
    /// نوع متریک قابل ثبت در Prometheus را مشخص می‌کند.
    /// </summary>
    public enum MetricType
    {
        Counter,
        Gauge,
        Histogram
    }
}
