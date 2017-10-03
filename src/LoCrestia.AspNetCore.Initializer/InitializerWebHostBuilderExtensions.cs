﻿using LoCrestia.AspNetCore.Initializer;
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
                services.AddSingleton<IWebHostInitializerService, WebHostInitializerService>(sp => new WebHostInitializerService());
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
