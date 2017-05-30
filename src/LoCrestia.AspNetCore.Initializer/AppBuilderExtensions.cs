using LoCrestia.AspNetCore.Initializer;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static async Task RunInitializationsAsync(this IApplicationBuilder app, Action<InitializerOptions> optionsAction = null)
        {
            var svc = app.ApplicationServices.GetService(typeof(IInitializerService)) as IInitializerService;
            var options = svc.Options;
            optionsAction?.Invoke(options);
            await svc.InitAsync();
        }

    }
}
