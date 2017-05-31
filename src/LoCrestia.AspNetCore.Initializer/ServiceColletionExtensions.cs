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
            services.AddSingleton<IInitializerService, InitializerService>(sp =>
            {
                var scopeFactory = sp.GetService<IServiceScopeFactory>();
                var options = new InitializationTasksOptions(sp);
                optionsAction?.Invoke(options);
                return new InitializerService(options, sp, scopeFactory);
            });
        }
    }
}
