using LoCrestia.AspNetCore.Initializer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class InitializerWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseInitializer(this IWebHostBuilder builder, Action<InitializerOptions> optionsAction = null)
        {
            builder.ConfigureServices(services =>
            {
                var options = new InitializerOptions();
                optionsAction?.Invoke(options);
                services.AddSingleton<IInitializerService>(new InitializerService(options));
                services.AddSingleton<IStartupFilter>(new InitializerStartupFilter(options));
            });
            return builder;
        }
    }
}
