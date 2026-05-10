using Prometheus.Contracts;
using Prometheus.Services;

namespace Prometheus.Extensions
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
                //prometheus scrape endpoint
                endpoints.MapMetrics();

                //liveness
                endpoints.MapGet("/health/live", async context =>
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = "UP"
                    });
                });

                //readiness
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
