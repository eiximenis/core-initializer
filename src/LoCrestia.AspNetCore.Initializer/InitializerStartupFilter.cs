using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerStartupFilter : IStartupFilter
    {
        private readonly InitializerOptions _options;
        private readonly IServiceProvider _serviceProvider;
        public InitializerStartupFilter(IServiceProvider serviceProvider, InitializerOptions options)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<InitializerMiddleware>(_options, _serviceProvider);
                next(app);
            };
        }
    }
}
