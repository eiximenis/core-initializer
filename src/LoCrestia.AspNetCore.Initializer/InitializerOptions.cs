using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerOptions
    {
        private readonly List<Func<Task>> _tasks;

        public int StatusCode { get; set; }

        internal bool Ready { get; private set; }
        
        public InitializerOptions()
        {
            _tasks = new List<Func<Task>>();
            Ready = false;
            StatusCode = 503;
            ErrorText = "Service is starting...";
        }

        internal IEnumerable<Func<Task>> Tasks => _tasks;

        public string ErrorText { get; set; }

        public InitializerOptions AddTask(Func<Task> task)
        {
            _tasks.Add(task);
            return this;
        }

        internal void FinishInitialization()
        {
            Ready = true;
        }

    }
}