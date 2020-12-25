using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntX.Middlewares
{
    public class GlobalLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger _logger;

        public GlobalLoggerMiddleware(RequestDelegate next, ILogger<GlobalLoggerMiddleware> logger)
        {
            this._logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();
                }
                await next(context);
                //var sb = await context.GetRequestContentAsync();
                //_logger.LogInformation(sb);
            }
            catch (Exception ex)
            {
                var sb = await context.GetRequestContentAsync();
                _logger.LogError(ex, $"{ex.Message}, 参数:{sb}");
                context.Response.StatusCode = 400;
            }
        }
    }

    public static class RequestLoggerHandlingExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalLoggerMiddleware>();
        }

        public static async Task<string> GetRequestContentAsync(this HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Method:{0}, Path:{1}", context.Request.Method, context.Request.Path);
            //foreach(var h in context.Request.Headers)
            //{
            //    sb.AppendFormat(", Header:{0}-{1}", h.Key, h.Value);
            //}
            if (context.Request.QueryString.HasValue)
            {
                sb.AppendFormat(", Query:{0}", context.Request.QueryString);
            }
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                // Leave the body open so the next middleware can read it.
                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 10240,
                    leaveOpen: true))
                {
                    string bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    sb.AppendFormat(", Body:{0}", bodyStr);
                }
            }
            return sb.ToString();
        }
    }
}
