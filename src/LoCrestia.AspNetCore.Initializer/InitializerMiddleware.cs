using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly InitializerOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public InitializerMiddleware(RequestDelegate next, InitializerOptions options, IServiceProvider serviceProvider)
        {
            _next = next;
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var svc = _serviceProvider.GetService(typeof(IInitializerService)) as IInitializerService;

            if (svc.HasFinished)
            {
                await _next.Invoke(context);
            }
            else
            {
                var response = context.Response;
                context.Response.StatusCode = _options.StatusCode;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(_options.ErrorText);
            }
        }


    }
}
