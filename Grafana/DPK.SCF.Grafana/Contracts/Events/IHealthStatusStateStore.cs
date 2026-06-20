namespace DPK.SCF.Grafana.Contracts.Events
{
    public interface IHealthStatusStateStore
    {
        bool HasChanged(string serviceName, string currentStatus);
    }
}
