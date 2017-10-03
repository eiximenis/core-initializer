using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public interface IWebHostTasksOptions
    {
        IInitializationTaskSettings AddTask(string name, Func<Task> task);
    }
}
