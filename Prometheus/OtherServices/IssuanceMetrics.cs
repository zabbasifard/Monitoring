using Prometheus.Contracts;

namespace Prometheus.OtherServices
{
    public class IssuanceMetrics
    {
        private readonly Counter _issued;
        private readonly Counter _failed;
        private readonly Counter _rejected;

        public IssuanceMetrics(IMetricsRegistery metricsRegistery, Counter failed, Counter rejected)
        {
            _issued = metricsRegistery.CreateCounter("", "", "");
            _failed = metricsRegistery.CreateCounter("", "", "");
            _rejected = metricsRegistery.CreateCounter("", "", "");
        }

        public void CertificateIssued() => _issued.Inc();
        public void CertificateFailed() => _failed.Inc();
        public void CertificateRejected() => _rejected.Inc();

    }
}
