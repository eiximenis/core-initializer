using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public class WebHostActionTask : ActionTask
    {
        private readonly IWebHost _webhost;
        public WebHostActionTask(IWebHost webhost, Func<Task> action) : base(action)
        {
            _webhost = webhost;
        }

        public async override Task RunAsync()
        {
            using (var scope = _webhost.Services.CreateScope())
            {
                await base.RunAsync();
            }
        }
    }
}
