using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Initializer.DemoWeb
{
    public class MyCustomTask
    {
        private readonly ILogger _logger;
        public MyCustomTask(ILogger<MyCustomTask> logger)
        {
            _logger = logger;
        }

        public async Task Run()
        {
            await Task.Delay(10000);
        }
    }
}
