namespace DPK.SCF.Grafana.Models
{
    public class ResourceMetricsOptions
    {
        public bool Enabled { get; set; } = true;

        public int CollectionIntervalSeconds { get; set; } = 15;
    }
}
