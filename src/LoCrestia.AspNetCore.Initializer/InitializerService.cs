using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerService : IInitializerService
    {
        private readonly InitializationTasksOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitializerService(InitializationTasksOptions options, IServiceProvider sp, IServiceScopeFactory scopeFactory)
        {
            _serviceProvider = sp;
            _serviceScopeFactory = scopeFactory;
            _options = options;
        }

        public bool HasFinished { get; private set; }

        public async Task InitAsync()
        {
            foreach (var task in _options.Tasks)
            {
                try
                {
                    await task.RunAsync();
                }
                catch
                {
                    task.MarkAsFailed();
                }

                if (!task.ContinueOnError && task.HasError)
                {
                    break;
                }
            }

            HasFinished = true;
        }
    }
}
