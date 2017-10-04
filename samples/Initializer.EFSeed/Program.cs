using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Initializer.EFSeed.Data;
using Microsoft.EntityFrameworkCore;

namespace Initializer.EFSeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .RunInitTasks(opt =>
                {
                    opt.AddTask<MyContext>("EF Seed", async (ctx) =>
                    {
                        await Task.Delay(20000);
                        ctx.Database.Migrate();
                        for (var i = 0; i < 1000; i++)
                        {
                            ctx.MyEntities.Add(new MyEntity() { Name = $"Test {i}" });
                        }
                        await ctx.SaveChangesAsync();
                    });
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
