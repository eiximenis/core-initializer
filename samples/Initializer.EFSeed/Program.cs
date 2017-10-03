using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace Initializer.EFSeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .AddPreBuildTasks(opt =>
                {
                    opt.AddTask("Test delay Task", () => Task.Delay(50000));
                })
                .Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseInitializer(options =>
            {
                options.ErrorText = "Doing some stuff";
                options.ResultPath = "/initresult";
            })
            .UseStartup<Startup>()
            .Build();
    }
}
