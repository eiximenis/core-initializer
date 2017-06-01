using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoCrestia.AspNetCore.Initializer.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerService : IInitializerService
    {
        private readonly InitializationTasksOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitializerResult Result { get; }

        public InitializerService(InitializationTasksOptions options, IServiceProvider sp, IServiceScopeFactory scopeFactory)
        {
            _serviceProvider = sp;
            _serviceScopeFactory = scopeFactory;
            _options = options;
            Result = new InitializerResult();
        }

        public async Task InitAsync()
        {
            Result.HasStarted = true;

            foreach (var task in _options.Tasks)
            {
                try
                {
                    Result.Running = task.Name;
                    await task.RunAsync();
                }
                catch (Exception ex)
                {
                    task.MarkAsFailed(ex);
                }

                var stop = NeedsToStopAfterTask(task);
                if (stop) { break; }
            }


            Result.SetAllTasks(_options.Tasks);
            Result.Running = null;
            Result.HasFinished = true;
        }

        private bool NeedsToStopAfterTask(IInitializationTask task)
        {
            
            if (task.HasError)
            {
                if (task.ThrowOnError)
                {
                    Result.SetAllTasks(_options.Tasks);
                    Result.HasFinished = true;
                    Result.Running = null;
                    throw task.Exception;
                }

                return !task.ContinueOnError;
            }

            if (task.Status == TaskResultStatus.Skipped)
            {
                return !task.CanBeSkipped;
            }

            return false;
        }
    }
}
