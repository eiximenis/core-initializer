using System;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public interface IInitializationTasksOptions
    {
        IInitializationTaskSettings AddTask(Func<Task> task);
        IInitializationTaskSettings AddTask(string name, Func<Task> task);
        IInitializationTaskSettings AddTask<T>(string name = null) where T : class;
    }

    public interface IInitializationTaskSettings
    {
        IInitializationTaskSettings CanBeSkipped();
        IInitializationTaskSettings ContinueOnError();
        IInitializationTaskSettings ThrowOnError();
    }
}