using Dpk.Observability.Middleware;
using Dpk.Observability.Observability.Contracts;
using Dpk.Observability.Observability.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace Dpk.Observability
{
    public static class Extensions
    {
        public static IServiceCollection AddCompanyObservability(this IServiceCollection services)
        {
            services.AddSingleton<HealthCheckRegistry>();
            services.AddSingleton<IMetricsRegistery, MetricRegistery>();

            return services;
        }

        public static IApplicationBuilder UseCompanyObservability(this IApplicationBuilder app)
        {
            app.UseHttpMetrics();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //prometheus scrape endpoint --وضعیت و آمار سیستم
                endpoints.MapMetrics();

                //liveness---زنده بودن
                endpoints.MapGet("/health/live", async context =>
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = "UP"
                    });
                });

                //readiness--آماده به کار
                endpoints.MapGet("/health/ready", async context =>
                {
                    var registry = context.RequestServices.GetRequiredService<HealthCheckRegistry>();

                    var result = await registry.CheckAllAsync();

                    if (result.Status == "DOWN")
                    {
                        context.Response.StatusCode = 503;
                    }

                    await context.Response.WriteAsJsonAsync(result);
                });
            });

            return app;
        }
    }
}
