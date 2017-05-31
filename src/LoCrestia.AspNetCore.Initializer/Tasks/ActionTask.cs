using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks
{
    public class ActionTask : InitializationTaskBase
    {
        public Func<Task> _action;

        public ActionTask(Func<Task> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            Status = TaskResultStatus.Pending;
        }

        public override async Task RunAsync()
        {
            await _action();
            Status = TaskResultStatus.Finished;
        }
    }
}
