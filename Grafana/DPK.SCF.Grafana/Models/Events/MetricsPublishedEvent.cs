namespace DPK.SCF.Grafana.Models.Events
{
    /// <summary>
    /// رویداد انتشار مقدار متریک جهت استفاده توسط ابزارهای مانیتورینگ و هشدار.
    /// </summary>
    public class MetricsPublishedEvent
    {
        public string ServiceName { get; set; } = default!;
        public string MetricName { get; set; } = default!;
        public double Value { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
