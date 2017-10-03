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
        public static async Task RunInitializationsAsync(this IApplicationBuilder app)
        {
            var svc = app.ApplicationServices.GetService(typeof(IStartupInitializerService)) as IStartupInitializerService;
            await svc.InitAsync();
        }

    }
}
