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
        public InitializerStartupFilter(InitializerOptions options) => _options = options;
       
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<InitializerMiddleware>(_options);
                next(app);
            };
        }
    }
}
