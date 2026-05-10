namespace Prometheus.Extensions
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const string header = "x-correlation-id";

            var correlationId =
                context.Request.Headers[header].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            context.Response.Headers[header] = correlationId;

            context.Items[header] = correlationId;

            await _next(context);
        }
    }
}
