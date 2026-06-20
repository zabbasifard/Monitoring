namespace DPK.SCF.Grafana.Entities
{
    public class SystemMetric
    {
        public long Id { get; set; }
        public string ServiceId { get; set; } = default!;
        public string MetricType { get; set; } = default!;
        public double Value { get; set; }
        public string Unit { get; set; } = default!;
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> Labels { get; set; } = new();

    }
}
