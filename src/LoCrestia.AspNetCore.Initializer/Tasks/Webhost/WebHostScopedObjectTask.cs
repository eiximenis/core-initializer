using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public class WebHostScopedObjectTask<T> : WebHostTaskBase
    {
        private readonly Func<T, Task> _action;

        public WebHostScopedObjectTask(IHost host, Func<T, Task> action) : base(host) => _action = action;

        public async override Task RunAsync()
        {
            using (var scope = Host.Services.CreateScope())
            {
                var param = scope.ServiceProvider.GetService<T>();
                await _action(param);
                Status = TaskResultStatus.Finished;
            }
        }

    }
}
