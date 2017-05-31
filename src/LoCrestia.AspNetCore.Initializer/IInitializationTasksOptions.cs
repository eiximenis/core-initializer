using System;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public interface IInitializationTasksOptions
    {
        IInitializationTaskSettings AddTask(Func<Task> task);
        IInitializationTaskSettings AddTask<T>() where T : class;
    }

    public interface IInitializationTaskSettings
    {
        IInitializationTaskSettings CanBeSkipped();
        IInitializationTaskSettings ContinueOnError();
    }
}