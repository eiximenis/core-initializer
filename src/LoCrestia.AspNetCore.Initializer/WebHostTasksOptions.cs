using LoCrestia.AspNetCore.Initializer.Tasks;
using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Extensions.Hosting;

namespace LoCrestia.AspNetCore.Initializer
{
    public class WebHostTasksOptions : IWebHostTasksOptions, IInitializationTaskSettings, IEnumerable<IInitializationTask>
    {
        private readonly IHost _host;
        private readonly List<WebHostTaskBase> _tasks;
        public WebHostTaskBase _lastTaskAdded;


        public WebHostTasksOptions(IHost host)
        {
            _host = host;
            _tasks = new List<WebHostTaskBase>();
            _lastTaskAdded = null;
        }

        public IInitializationTaskSettings AddTask(string name, Func<IServiceScope, Task> task)
        {
            var webHostTask = new WebHostActionTask(_host, task);
            _tasks.Add(webHostTask);
            webHostTask.Name = name ?? $"WebHost task #{_tasks.Count}";
            _lastTaskAdded = webHostTask;
            return this;
        }

        public IInitializationTaskSettings AddTask<T>(string name, Func<T, Task> task)
        {
            var webHostTask = new WebHostScopedObjectTask<T>(_host, task);
            _tasks.Add(webHostTask);
            webHostTask.Name = name ?? $"WebHostScopedObject task #{_tasks.Count}";
            _lastTaskAdded = webHostTask;
            return this;
        }

        public IInitializationTaskSettings CanBeSkipped()
        {
            _lastTaskAdded.CanBeSkipped = true;
            return this;
        }

        public IInitializationTaskSettings ContinueOnError()
        {
            _lastTaskAdded.ContinueOnError = true;
            return this;
        }

        public IInitializationTaskSettings ThrowOnError()
        {
            _lastTaskAdded.ThrowOnError = true;
            return this;
        }

        public IEnumerator<IInitializationTask> GetEnumerator()
        {
            foreach (var task in _tasks)
            {
                yield return task;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
