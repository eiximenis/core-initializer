using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;

namespace LoCrestia.AspNetCore.Initializer
{
    public interface IInitializerService
    {
        InitializerResult Result { get; }
    }
}
