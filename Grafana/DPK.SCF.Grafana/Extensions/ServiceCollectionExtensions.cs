using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Contracts.Events;
using DPK.SCF.Grafana.Events;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DPK.SCF.Grafana.Extensions
{
    /// <summary>
    /// متدهای توسعه‌ای برای ثبت سرویس‌های مانیتورینگ در DI.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScfMonitoring(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MonitoringOptions>(configuration.GetSection("Monitoring"));

            services.Configure<HealthMonitoringOptions>(configuration.GetSection("HealthMonitoring"));

            services.Configure<ResourceMetricsOptions>(configuration.GetSection("ResourceMetrics"));

            services.AddSingleton<IMetricsManager, MetricsManager>();

            services.AddScoped<IHealthCheckEngine, HealthCheckEngine>();

            services.AddHttpClient<IHealthAggregatorService, HealthAggregatorService>();

            services.AddScoped<IMetricsCollectorService, MetricsCollectorService>();

            services.TryAddScoped<IHealthEventPublisher, NoOpHealthEventPublisher>();

            services.TryAddScoped<IMetricsEventPublisher, NoOpMetricsEventPublisher>();

            services.AddSingleton<IHealthStatusStateStore, InMemoryHealthStatusStateStore>();

            services.AddHostedService<SystemResourceMetricsBackgroundService>();


            services
                .AddHealthChecks()
                .AddCheck(
                    "self",
                    () => HealthCheckResult.Healthy("Application is running."),
                    tags: new[] { "live" });

            return services;
        }
    }
}