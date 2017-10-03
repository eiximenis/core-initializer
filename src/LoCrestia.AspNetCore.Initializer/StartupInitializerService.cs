using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoCrestia.AspNetCore.Initializer.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class StartupInitializerService : IStartupInitializerService
    {
        private readonly InitializationTasksOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IWebHostInitializerService _webHostInitializerSvc;

        public InitializerResult Result { get; }

        public StartupInitializerService(InitializationTasksOptions options, 
            IServiceProvider sp, 
            IServiceScopeFactory scopeFactory,
            IWebHostInitializerService webHostInitializerSvc)
        {
            _serviceProvider = sp;
            _serviceScopeFactory = scopeFactory;
            _options = options;
            _webHostInitializerSvc = webHostInitializerSvc;
            Result = _webHostInitializerSvc.Result;
        }

        public async Task InitAsync()
        {
            await _webHostInitializerSvc.WaitForPendingTasks();
            await new TasksRunner(_options.Tasks).RunAll(Result);
            
            Result.SetAllTasks(_options.Tasks);
            Result.Running = null;
            Result.HasFinished = true;
        }

        
    }
}
