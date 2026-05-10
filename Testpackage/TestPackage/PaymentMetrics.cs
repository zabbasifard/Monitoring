using Dpk.Observability.Observability.Contracts;
using Prometheus;

namespace TestPackage
{
    public class PaymentMetrics
    {
        private readonly Counter _success;
        private readonly Counter _failed;
        private readonly Counter _rejected;

        public PaymentMetrics(IMetricsRegistery metricsRegistery)
        {
            _success = metricsRegistery.CreateCounter("payment_success_total", "help", "payment_success_total");
            _failed = metricsRegistery.CreateCounter("payment_failed_total", "help", "payment_failed_total");
            _rejected = metricsRegistery.CreateCounter("payment_reject_total", "help", "payment_reject_total");

        }

        public void PaymentSuccess() => _success.Inc();
        public void PaymentFailed() => _failed.Inc();
        public void PaymentRejected() => _rejected.Inc();
    }
}
