using LoCrestia.AspNetCore.Initializer.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class TasksRunner
    {
        private readonly IEnumerable<IInitializationTask> _tasks;
        public TasksRunner(IEnumerable<IInitializationTask> tasks)
        {
            _tasks = tasks;
        }

        public async Task RunAll(InitializerResult result)
        {
            foreach (var task in _tasks)
            {
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
            }
        }

        private bool NeedsToStopAfterTask(IInitializationTask task, InitializerResult result)
        {

            if (task.HasError)
            {
                if (task.ThrowOnError)
                {
                    result.SetAllTasks(_tasks);
                    result.HasFinished = true;
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
