namespace Prometheus.Extensions
{

    public class HealthCheckRegistry
    {
        private readonly Dictionary<string, Func<Task<bool>>> _checks = new();

        public void Register(string name, Func<Task<bool>> check)
        {
            _checks[name] = check;
        }

        public async Task<HealthResult> CheckAllAsync()
        {
            var services = new Dictionary<string, string>();

            foreach (var check in _checks)
            {
                try
                {
                    var ok = await check.Value();

                    services[check.Key] = ok ? "UP" : "DOWN";
                }
                catch
                {
                    services[check.Key] = "DOWN";
                }
            }

            var allUp = services.Values.All(v => v == "UP");

            return new HealthResult
            {
                Status = allUp ? "UP" : "DOWN",
                Services = services
            };
        }
    }

    public class HealthResult
    {
        public string Status { get; set; } = "UP";

        public Dictionary<string, string> Services { get; set; } = new();
    }
}
