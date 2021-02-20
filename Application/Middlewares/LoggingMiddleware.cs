using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Entities.Identity;

namespace Application.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _start;


        public LoggingMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
            _start = Stopwatch.GetTimestamp();
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService)
        {

            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var userId = userService.GetAuthorizedUserId(httpContext.User);
            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("UserId", userId);

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning

            await RequestLog(httpContext.Request).ConfigureAwait(false);


            // The reponse body is also a stream so we need to:
            // - hold a reference to the original response body stream
            // - re-point the response body to a new memory stream
            // - read the response body after the request is handled into our memory stream
            // - copy the response in the memory stream out to the original response stream
            await ResponseLog(httpContext).ConfigureAwait(false);
        }

        private async Task RequestLog(HttpRequest request)
        {
            var requestBody = "";


            var requestSizeLimit = 0;
            if (request.ContentLength != null) requestSizeLimit = (int)request.ContentLength.Value;

            request.EnableBuffering();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;




            Log.ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestBody", requestBody).Information("Request information {RequestMethod} {RequestPath}",
                    request.Method, request.Path);
        }

        private async Task ResponseLog(HttpContext httpContext)
        {
            var response = httpContext.Response;

            var responseBody = "";


            var originalBody = httpContext.Response.Body;
            var uri = new Uri(httpContext.Request.GetEncodedUrl());

            try
            {

                await _next(httpContext).ConfigureAwait(false);


                await using var memStream = new MemoryStream();
                httpContext.Response.Body = memStream;
                memStream.Position = 0;
                responseBody = await new StreamReader(memStream).ReadToEndAsync().ConfigureAwait(false);
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody).ConfigureAwait(false);


                Log.ForContext("ResponseHeaders", response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()))
                    .ForContext("ResponseBody", responseBody)
                    .Information("Response information {RequestMethod} {RequestPath} {statusCode} {ElapsedTime} s",
                        httpContext.Request.Method, httpContext.Request.Path, response.StatusCode,
                        GetElapsedMilliseconds(_start, Stopwatch.GetTimestamp()));

            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (double)(((stop - start) * 1000L) / (double)Stopwatch.Frequency);
        }
    }
}
