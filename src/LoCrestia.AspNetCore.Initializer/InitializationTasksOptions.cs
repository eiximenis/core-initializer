using LoCrestia.AspNetCore.Initializer.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializationTasksOptions : IInitializationTasksOptions, IInitializationTaskSettings
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

        public IEnumerable<IInitializationTask> Tasks => _tasks;



        public IInitializationTaskSettings AddTask(Func<Task> task)
        {
            _lastTaskAdded = new ActionTask(task);
            _tasks.Add(_lastTaskAdded);
            return this;
        }

        public IInitializationTaskSettings AddTask<T>() where T : class
        {
            _lastTaskAdded = new ObjectTask<T>(_serviceProvider);
            _tasks.Add(_lastTaskAdded);
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

    }
}