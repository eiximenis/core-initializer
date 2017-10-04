using LoCrestia.AspNetCore.Initializer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerResult
    {
        public class TaskResult
        {
            public string Name { get; set; }
            public bool HasError { get; set; }
            public TaskResultStatus Status { get; set; }
            public string StatusText => Enum.GetName(typeof(TaskResultStatus), Status);
            public string ErrorDescription { get; set; }
        }

        private readonly List<TaskResult> _results;
        private int _pendingServices;
        public bool HasStarted => _pendingServices > 0;
        public bool HasFinished => _pendingServices == 0;
        public IEnumerable<TaskResult> Results => _results;

        public void Start()
        {
            _pendingServices++;
        }

        public void Stop()
        {
            _pendingServices--;
        }


        public string Running { get; internal set; }

        public InitializerResult()
        {
            _results = new List<TaskResult>();
            _pendingServices = 0;
        }

        public void AddResultTasks(IEnumerable<IInitializationTask> tasks)
        {
            _results.AddRange(tasks.Select(t => new TaskResult()
            {
                Name = t.Name,
                Status = t.Status,
                HasError = t.HasError,
                ErrorDescription = GetErrorDescription(t.Exception)
            }));
        }

        private string GetErrorDescription(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }
            return $"[Exception] {exception.GetType().Name}: {exception.Message}\n\n{exception.StackTrace}";
        }
    }
}
