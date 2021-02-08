using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoCrestia.AspNetCore.Initializer.Tasks.Webhost
{
    public abstract class WebHostTaskBase : InitializationTaskBase
    {
        protected IHost Host{ get; }

        public WebHostTaskBase(IHost host) => Host = host;


    }
}
