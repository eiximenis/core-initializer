using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerService : IInitializerService
    {
        public InitializerOptions Options { get; }

        public InitializerService(InitializerOptions options) => Options = options;

        public async Task InitAsync()
        {
            foreach (var task in Options.Tasks)
            {
                await task.Invoke();
            }

            Options.FinishInitialization();
        }
    }
}
