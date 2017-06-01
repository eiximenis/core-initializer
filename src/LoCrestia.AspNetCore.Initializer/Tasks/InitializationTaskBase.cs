using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks
{
    public abstract class InitializationTaskBase : IInitializationTask
    {
        public TaskResultStatus Status { get; protected set; }
        public bool CanBeSkipped { get; set; }
        public bool ContinueOnError { get; set; }
        public bool ThrowOnError { get; set; }

        public string Name { get; set; }

        public bool HasError => Status == TaskResultStatus.FinishedWithError;

        public Exception Exception { get; private set; }

        public InitializationTaskBase()
        {
            CanBeSkipped = false;
            ThrowOnError = false;
            ContinueOnError = false;
        }

        public void MarkAsFailed(Exception ex)
        {
            Exception = ex;
            Status = TaskResultStatus.FinishedWithError;
        }

        public abstract Task RunAsync();
    }
}
