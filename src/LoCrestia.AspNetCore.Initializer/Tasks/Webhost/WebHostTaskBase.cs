using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public abstract class WebHostTaskBase : InitializationTaskBase
    {
        protected IWebHost WebHost { get; }

        public WebHostTaskBase(IWebHost host) => WebHost = host;


    }
}
