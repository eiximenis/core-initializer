using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks
{
    public interface IInitializationTask
    {
        TaskResultStatus Status { get; }
        bool CanBeSkipped { get; set; }
        bool ContinueOnError { get; set; }
        bool ThrowOnError { get; set; }
        bool HasError { get; }

        string Name { get; }
        Exception Exception { get;  }

        Task RunAsync();
        void MarkAsFailed(Exception ex);
    }
}
