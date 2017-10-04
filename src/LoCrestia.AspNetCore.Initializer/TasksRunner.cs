using LoCrestia.AspNetCore.Initializer.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class TasksRunner
    {
        private readonly List<IInitializationTask> _tasks;
        private readonly object _syncObject;
        public TasksRunner()
        {
            _syncObject = new object();
            _tasks = new List<IInitializationTask>();
        }

        public void AppendTasks(IEnumerable<IInitializationTask> additionalTasks)
        {
            lock (_syncObject)
            {
                foreach (var task in additionalTasks)
                {
                    _tasks.Add(task);
                }
            }
        }

        public async Task RunAll(InitializerResult result)
        {
            if (!_tasks.Any())
            {
                return;
            }

            bool pending = true;
            int idx = 0;
            
            while (pending)
            {
                var task = _tasks[idx];
                try
                {
                    result.Running = task.Name;
                    await task.RunAsync();
                }
                catch (Exception ex)
                {
                    task.MarkAsFailed(ex);
                }

                var stop = NeedsToStopAfterTask(task, result);
                if (stop) { break; }
 
                idx++;
                lock (_syncObject)
                {
                    pending = idx < _tasks.Count;
                }
            }
        }

        private bool NeedsToStopAfterTask(IInitializationTask task, InitializerResult result)
        {

            if (task.HasError)
            {
                if (task.ThrowOnError)
                {
                    result.AddResultTasks(_tasks);
                    result.Stop();
                    result.Running = null;
                    throw task.Exception;
                }

                return !task.ContinueOnError;
            }

            if (task.Status == TaskResultStatus.Skipped)
            {
                return !task.CanBeSkipped;
            }

            return false;
        }
    }
}
