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
        bool HasError { get; }

        Task RunAsync();
        void MarkAsFailed();
    }
}
