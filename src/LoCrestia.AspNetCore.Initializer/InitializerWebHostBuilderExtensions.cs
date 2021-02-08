using LoCrestia.AspNetCore.Initializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class InitializerWebHostBuilderExtensions
    {
        public static IHostBuilder UseInitializer(this IHostBuilder builder, Action<InitializerOptions> optionsAction = null)
        {

            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IInitializerService, InitializerService>();
            });

            builder.ConfigureServices(services =>
            {
                var options = new InitializerOptions();
                optionsAction?.Invoke(options);
                services.AddSingleton<IStartupFilter>(sp => new InitializerStartupFilter(sp, options));
            });
            return builder;
        }


    }
}
