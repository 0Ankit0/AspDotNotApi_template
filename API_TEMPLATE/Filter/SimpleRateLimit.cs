using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;

namespace API_TEMPLATE.Filter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SimpleRateLimitAttribute : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<string, (DateTime lastRequestTime, int requestCount)> _clients = new();
        private readonly int _maxRequests;
        private readonly TimeSpan _timeSpan;

        public SimpleRateLimitAttribute(int maxRequests, int seconds)
        {
            _maxRequests = maxRequests;
            _timeSpan = TimeSpan.FromSeconds(seconds);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clientIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (clientIp == null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var now = DateTime.UtcNow;
            var clientInfo = _clients.GetOrAdd(clientIp, _ => (now, 0));

            if (now - clientInfo.lastRequestTime > _timeSpan)
            {
                // Reset the rate limit counter
                _clients[clientIp] = (now, 1);
            }
            else
            {
                if (clientInfo.requestCount >= _maxRequests)
                {
                    // Calculate remaining time
                    var resetTime = clientInfo.lastRequestTime + _timeSpan;
                    var retryAfter = (int)(resetTime - now).TotalSeconds;

                    context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();

                    context.Result = new ContentResult
                    {
                        StatusCode = 429, // Too Many Requests
                        Content = $"Rate limit exceeded. Try again in {retryAfter} seconds."
                    };
                    return;
                }

                // Increment the request count
                _clients[clientIp] = (clientInfo.lastRequestTime, clientInfo.requestCount + 1);
            }

            base.OnActionExecuting(context);
        }
    }
}