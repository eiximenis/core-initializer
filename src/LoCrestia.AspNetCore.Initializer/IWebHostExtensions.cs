using LoCrestia.AspNetCore.Initializer;
using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Hosting
{
    public static class IWebHostExtensions
    {
        public static IWebHost RunPreBuildTask(this IWebHost webHost, Action<IWebHost> actionToDo)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                actionToDo?.Invoke(webHost);
            }
            return webHost;
        }


        public static IWebHost AddPreBuildTasks(this IWebHost webHost, Action<IWebHostTasksOptions> optionsAction)
        {

            var initService = webHost.Services.GetRequiredService<IWebHostInitializerService>();

            var options = new WebHostTasksOptions(webHost);
            optionsAction?.Invoke(options);
            initService.SetOptions(options);
            return webHost;
        }
    }
}
