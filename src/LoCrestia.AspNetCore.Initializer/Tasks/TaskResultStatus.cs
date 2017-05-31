using System;
using System.Collections.Generic;
using System.Text;

namespace LoCrestia.AspNetCore.Initializer.Tasks
{
    public enum TaskResultStatus
    {
        Pending,
        Finished,
        FinishedWithError,
        Skipped
    }
}
