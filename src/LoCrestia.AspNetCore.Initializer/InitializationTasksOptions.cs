using LoCrestia.AspNetCore.Initializer.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializationTasksOptions : IInitializationTasksOptions, IInitializationTaskSettings, IEnumerable<IInitializationTask>
    {
        private readonly List<IInitializationTask> _tasks;
        private readonly IServiceProvider _serviceProvider;
        private IInitializationTask _lastTaskAdded;

        public InitializationTasksOptions(IServiceProvider serviceProvider)
        {
            _tasks = new List<IInitializationTask>();
            _serviceProvider = serviceProvider;
            _lastTaskAdded = null;
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


        public IInitializationTaskSettings AddTask(Func<Task> task)
        {
            return AddTask(null, task);
        }

        public IInitializationTaskSettings AddTask(string name, Func<Task> task)
        {
            var inittask = new ActionTask(task);
            _tasks.Add(inittask);
            inittask.Name = name ?? $"Func Task #{_tasks.Count}";
            _lastTaskAdded = inittask;
            return this;
        }

        public IInitializationTaskSettings AddTask<T>(string name = null) where T : class
        {
            var inittask = new ObjectTask<T>(_serviceProvider);
            _tasks.Add(inittask);
            inittask.Name = name ?? $"Object Task #{_tasks.Count}";
            _lastTaskAdded = inittask;
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