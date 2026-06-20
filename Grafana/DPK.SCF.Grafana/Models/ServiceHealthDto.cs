namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// نتیجه بررسی سلامت یک سرویس شامل وضعیت کلی، زمان بررسی و وضعیت وابستگی‌ها.
    /// </summary>
    public class ServiceHealthDto
    {
        public string ServiceId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CheckedAt { get; set; }
        public double LatencyMs { get; set; }
        public List<ComponentHealthDto> Components { get; set; } = new();
    }
}
