using Serilog;
using System.Text;
using Serilog.Context;
using System.Diagnostics;

namespace JwtWithIdentity.CustomMiddleware;

public class CentralLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public CentralLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;
        var requestBody = await ReadRequestBody(context);

        // CorrelationId (yoxdursa yaradırıq)
        var correlationId = context.Request.Headers["X-Correlation-Id"]
            .FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers["X-Correlation-Id"] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("RequestPath", request.Path))
        using (LogContext.PushProperty("RequestMethod", request.Method))
        {
            Log.Information("Incoming Request {@RequestBody}", requestBody);

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled Exception");
                throw;
            }

            stopwatch.Stop();

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            Log.Information("Outgoing Response {StatusCode} in {ElapsedMilliseconds} ms {@ResponseBody}",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(
            context.Request.Body,
            Encoding.UTF8,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        return body;
    }
}
