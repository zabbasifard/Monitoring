namespace DPK.SCF.Grafana.Models
{
    /// <summary>
    /// وضعیت سلامت یک وابستگی یا مؤلفه داخلی/خارجی سرویس را نگهداری می‌کند.
    /// </summary>
    public class ComponentHealthDto
    {
        public string Name { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? Message { get; set; }
    }
}
