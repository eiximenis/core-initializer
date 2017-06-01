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
        public bool HasStarted { get; set; }
        public bool HasFinished { get; set; }
        public IEnumerable<TaskResult> Results => _results;

        public string Running { get; internal set; }

        public InitializerResult()
        {
            _results = new List<TaskResult>();
        }

        public void SetAllTasks(IEnumerable<IInitializationTask> tasks)
        {
            _results.Clear();
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
