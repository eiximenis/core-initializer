using LoCrestia.AspNetCore.Initializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceColletionExtensions
    {
        public static void AddInitTasks(this IServiceCollection services, Action<InitializationTasksOptions> optionsAction)
        {

            services.AddSingleton<IStartupInitializerService, StartupInitializerService>(sp =>
            {
                var webHostinitSvc = sp.GetRequiredService<IWebHostInitializerService>();
                var scopeFactory = sp.GetService<IServiceScopeFactory>();
                var options = new InitializationTasksOptions(sp);
                optionsAction?.Invoke(options);
                return new StartupInitializerService(options, sp, scopeFactory, webHostinitSvc);
            });
        }
    }
}
