using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Prometheus;

namespace DPK.SCF.Grafana.Extensions
{
    /// <summary>
    /// متدهای توسعه‌ای برای ثبت Endpointهای مانیتورینگ.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapScfMonitoringEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapMetrics();

            endpoints.MapHealthChecks(
                "/health/live",
                new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("live")
                });

            endpoints.MapHealthChecks(
                "/health/ready",
                new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("ready")
                });

            endpoints.MapGet(
               "/api/platform/health",
               async (IHealthCheckEngine healthCheckEngine, CancellationToken cancellationToken) =>
               {
                   var result = await healthCheckEngine.CheckAsync(cancellationToken);

                   if (result.Status == ServiceHealthStatus.Down)
                       return Results.Json(
                           result,
                           statusCode: StatusCodes.Status503ServiceUnavailable);

                   return Results.Ok(result);
               });

            endpoints.MapGet(
                 "/api/ops/health",
                 async (
                     string? serviceId,
                     IHealthAggregatorService healthAggregatorService,
                     CancellationToken cancellationToken) =>
                 {
                     try
                     {
                         var result = await healthAggregatorService.GetHealthAsync(
                             serviceId,
                             cancellationToken);

                         return Results.Ok(result);
                     }
                     catch
                     {
                         return Results.Problem(
                             title: "MONITORING_CONN_FAILED",
                             statusCode: StatusCodes.Status503ServiceUnavailable);
                     }
                 });

            endpoints.MapPost(
                 "/api/platform/metrics",
                 async (
                     CreateMetricRequest request,
                     IMetricsCollectorService metricsCollectorService,
                     CancellationToken cancellationToken) =>
                 {
                     await metricsCollectorService.CollectAsync(request, cancellationToken);

                     return Results.Ok(new
                     {
                         status = "registered"
                     });
                 });

            return endpoints;
        }

    }

}
