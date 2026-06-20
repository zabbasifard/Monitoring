using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Models.Constaints;
using System.Collections.Concurrent;

namespace DPK.SCF.Grafana.Services
{
    public class InMemoryHealthStatusStateStore : IHealthStatusStateStore
    {
        private readonly ConcurrentDictionary<string, string> _statuses = new();

        public bool HasChanged(string serviceName, string currentStatus)
        {
            if (!_statuses.TryGetValue(serviceName, out var previousStatus))
            {
                _statuses[serviceName] = currentStatus;
                return false;
            }

            if (previousStatus == currentStatus)
                return false;

            _statuses[serviceName] = currentStatus;
            return true;
        }

    }
}