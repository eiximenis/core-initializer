using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public class WebHostActionTask : WebHostTaskBase
    {
        private readonly Func<IServiceScope, Task> _action;
        public WebHostActionTask(IHost webhost, Func<IServiceScope, Task> action) : base(webhost) => _action = action;

        public async override Task RunAsync()
        {
            using (var scope = Host.Services.CreateScope())
            {
                await _action(scope);
                Status = TaskResultStatus.Finished;
            }
        }
    }
}
