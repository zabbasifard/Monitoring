namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// درخواست ثبت مقدار یک متریک از سمت سرویس‌های مختلف.
    /// </summary>
    public class CreateMetricRequest
    {
        public string ServiceId { get; set; } = default!;

        public string MetricType { get; set; } = default!;

        public double Value { get; set; }

        public string Unit { get; set; } = default!;
    }
}
