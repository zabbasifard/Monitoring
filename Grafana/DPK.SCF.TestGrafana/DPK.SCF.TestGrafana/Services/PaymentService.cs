using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models.Constaints;
using DPK.SCF.Grafana.Services;

namespace DPK.SCF.TestGrafana.Services
{
    public class PaymentService
    {
        private readonly IMetricsManager _metrics;

        public PaymentService(IMetricsManager metrics)
        {
            _metrics = metrics;
        }

        public async Task PayAsync()
        {
            //برای شمارش رخداد ها مثلا تعداد درخواست ها - تعداد موفق /ناموفق - تعداد خطاها
            _metrics.Increment(MetricDefinitions.RequestTotal, new Dictionary<string, string>
            {
                [MetricLabels.Service] = "PaymentService",
                [MetricLabels.Status] = "started"
            });

            //برای اندازه گیری مدت زمان اجرا عملیات
            using (_metrics.Measure(MetricDefinitions.Duration, new Dictionary<string, string> { [MetricLabels.Service] = "PaymentService" }))
            {
                await Task.Delay(500);

                _metrics.Increment(MetricDefinitions.RequestTotal, new Dictionary<string, string>
                {
                    [MetricLabels.Service] = "PaymentService",
                    [MetricLabels.Status] = "success"
                });

                //برای ثبت مقدار لحظه ای - تعداد کاربران آنلاین - تعداد کانکشن های فعال
                _metrics.SetGauge(MetricDefinitions.ActiveUsers, 120, new Dictionary<string, string>
                {
                    [MetricLabels.Service] = "PaymentService"
                });
            }
        }
    }
}
