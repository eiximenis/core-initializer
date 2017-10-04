﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public class WebHostActionTask : WebHostTaskBase
    {
        private readonly Func<IServiceScope, Task> _action;
        public WebHostActionTask(IWebHost webhost, Func<IServiceScope, Task> action) : base(webhost) => _action = action;

        public async override Task RunAsync()
        {
            using (var scope = WebHost.Services.CreateScope())
            {
                await _action(scope);
                Status = TaskResultStatus.Finished;
            }
        }
    }
}
