using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly InitializerOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly bool _resultEndpointAvailable;

        private bool _finished;

        public InitializerMiddleware(RequestDelegate next, InitializerOptions options, IServiceProvider serviceProvider)
        {
            _next = next;
            _options = options;
            _serviceProvider = serviceProvider;
            _resultEndpointAvailable = !string.IsNullOrWhiteSpace(options.ResultPath);
            _finished = false;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_resultEndpointAvailable)
            {
                var path = context.Request.Path;
                if (path.Equals(_options.ResultPath, StringComparison.OrdinalIgnoreCase))
                {
                    await ProcessResultRequest(context);
                    return;
                }
            }

            if (!_finished)
            {
                var svc = _serviceProvider.GetService(typeof(IInitializerService)) as IInitializerService;
                if (svc.Result.HasFinished)
                {
                    _finished = true;
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
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task ProcessResultRequest(HttpContext context)
        {
            var svc = _serviceProvider.GetService(typeof(IInitializerService)) as IInitializerService;
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            var message = JsonSerializer.Serialize(svc.Result);
            await context.Response.WriteAsync(message);
        }
    }
}
