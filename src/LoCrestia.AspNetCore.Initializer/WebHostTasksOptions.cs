using LoCrestia.AspNetCore.Initializer.Tasks.Webhost;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class WebHostTasksOptions : IWebHostTasksOptions, IInitializationTaskSettings
    {
        private readonly IWebHost _webHost;
        private readonly List<WebHostActionTask> _tasks;
        public WebHostActionTask _lastTaskAdded;

        public IEnumerable<WebHostActionTask> Tasks => _tasks;

        public WebHostTasksOptions(IWebHost webHost)
        {
            _webHost = webHost;
            _tasks = new List<WebHostActionTask>();
            _lastTaskAdded = null;
        }

        public IInitializationTaskSettings AddTask(string name, Func<Task> task)
        {
            var webHostTask = new WebHostActionTask(_webHost, task);
            webHostTask.Name = name ?? "WebHost task";
            _tasks.Add(webHostTask);
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
    }
}
