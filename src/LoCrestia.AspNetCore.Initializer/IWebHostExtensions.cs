using LoCrestia.AspNetCore.Initializer;
using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    public static class IWebHostExtensions
    {
        public static IHost RunPreBuildTask(this IHost host, Action<IHost> actionToDo)
        {
            using (var scope = host.Services.CreateScope())
            {
                actionToDo?.Invoke(host);
            }
            return host;
        }

        public static IHost RunInitTasks(this IHost host)
        {
            var initService = host.Services.GetService<IInitializerService>() as InitializerService;
            initService.Run().ConfigureAwait(false);
            return host;
        }

        public static IHost RunInitTasks(this IHost host, Action<IWebHostTasksOptions> optionsAction)
        {
            var initService = host.Services.GetService<IInitializerService>() as InitializerService;
            var options = new WebHostTasksOptions(host);
            optionsAction?.Invoke(options);
            initService.AddTasks(options);
            initService.Run().ConfigureAwait(false);
            return host;
        }
    }
}
