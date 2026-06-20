using DPK.SCF.Grafana.Models.Constaints;

namespace DPK.SCF.Grafana.Contracts
{
    /// <summary>
    /// قرارداد ثبت و مدیریت متریک‌های سامانه.
    /// </summary>
    public interface IMetricsManager
    {
        void Increment(MetricDefinition metric, Dictionary<string, string>? labels = null);

        void SetGauge(MetricDefinition metric, double value, Dictionary<string, string>? labels = null);

        IDisposable Measure(MetricDefinition metric, Dictionary<string, string>? labels = null);
    }
}
