using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace DPK.SCF.Grafana.Extensions
{
    /// <summary>
    /// متدهای توسعه‌ای برای فعال‌سازی Middlewareهای مانیتورینگ.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseScfMonitoring(
            this IApplicationBuilder app)
        {
            app.UseHttpMetrics();

            return app;
        }
    }
}

