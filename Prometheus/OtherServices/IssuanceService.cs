namespace Prometheus.OtherServices
{
    public class IssuanceService
    {
        private readonly IssuanceMetrics _metrics;
        public IssuanceService(IssuanceMetrics metrics)
        {
            _metrics = metrics;
        }

        public void Issue(string data)
        {
            //validation is not pass
            _metrics.CertificateRejected();
            try
            {
                //implement bussiness logic
                //success
                _metrics.CertificateIssued();
            }
            catch (Exception)
            {
                //unsuccess
                _metrics.CertificateFailed();
                throw;
            }
        }

    }
}
