namespace DPK.SCF.Grafana.Models.Events
{
    /// <summary>
    /// رویداد ثبت یک متریک از سمت سرویس ارسال‌کننده.
    /// </summary>
    public class MetricsCollectedEvent
    {
        public string ServiceId { get; set; } = default!;
        public string MetricType { get; set; } = default!;
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
