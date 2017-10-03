using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class WebHostInitializerService : IWebHostInitializerService
    {
        private WebHostTasksOptions _options;
        public InitializerResult Result { get; }
        private TasksRunner _runner;
        private object _syncObject;
        private SemaphoreSlim _semaphore;

        public WebHostInitializerService()
        {
            _options = null;
            _runner = null;
            _semaphore = new SemaphoreSlim(1);
            Result = new InitializerResult();
        }

        public  async Task WaitForPendingTasks()
        {
            await _semaphore.WaitAsync();
        }

        public void SetOptions(WebHostTasksOptions options)
        {
            _options = options;
            _runner = new TasksRunner(_options.Tasks);
            _semaphore.Wait();
            Result.HasStarted = true;
            _runner.RunAll(Result).ContinueWith(t =>
            {
                Result.SetAllTasks(_options.Tasks);
                Result.Running = null;
                Result.HasFinished = true;
                _semaphore.Release();
            });
        }
    }
}
