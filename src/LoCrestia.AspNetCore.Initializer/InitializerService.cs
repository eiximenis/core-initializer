using LoCrestia.AspNetCore.Initializer.Tasks;
using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerService : IInitializerService
    {
        public InitializerResult Result { get; }
        private readonly TasksRunner _runner;
        private readonly List<IInitializationTask> _tasks;

        public InitializerService()
        {
            Result = new InitializerResult();
            _runner = new TasksRunner();
            _tasks = new List<IInitializationTask>();
        }

        public void AddTasks(IEnumerable<IInitializationTask> tasks)
        {
            _tasks.AddRange(tasks);
            _runner.AppendTasks(tasks);
        }

        public async Task Run()
        {

            Result.Start();
            await _runner.RunAll(Result);
            Result.AddResultTasks(_tasks);
            Result.Running = null;
            Result.Stop();


        }
    }
}
