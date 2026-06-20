namespace DPK.SCF.Grafana.Models.Events
{
    /// <summary>
    /// رویداد ثبت نتیجه بررسی سلامت سرویس.
    /// </summary>
    public class HealthCheckedEvent
    {
        public string ServiceId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CheckedAt { get; set; }
    }
}

