using Serilog;
using Serilog.Context;

namespace Presentation.Middlewares
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {

            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = System.Guid.NewGuid().ToString();
            }


            context.Response.Headers.Append(CorrelationIdHeader, correlationId);

            // Log the correlation ID and add it to the Serilog context
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                Log.Information("Processing request with Correlation ID: {CorrelationId}", correlationId);
                await _next(context);
            }
        }
    }
}
