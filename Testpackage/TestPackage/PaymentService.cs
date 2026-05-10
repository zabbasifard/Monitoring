namespace TestPackage
{
    public class PaymentService
    {
        private readonly PaymentMetrics _metrics;
        public PaymentService(PaymentMetrics metrics)
        {
            _metrics = metrics;
        }

        public void pay()
        {
            try
            {
                //payment logic
                _metrics.PaymentSuccess();
            }
            catch (Exception)
            {
                _metrics.PaymentFailed();
                throw;
            }
        }
    }
}
