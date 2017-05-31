using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer.Tasks
{
    public class ObjectTask<T> : InitializationTaskBase
        where T : class
    {
        private readonly IServiceProvider _provider;
        public ObjectTask(IServiceProvider provider)
        {
            _provider = provider;
            Status = TaskResultStatus.Pending;
        }

        public override async Task RunAsync()
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            var scope = _provider.CreateScope();
            var method = typeInfo.GetDeclaredMethod("Run");
            var hasRunMethod = method != null;
            if (hasRunMethod)
            {
                await RunTaskAsync(type, scope, method).ConfigureAwait(false);
                Status = TaskResultStatus.Finished;
            }
            else
            {
                Status = TaskResultStatus.Skipped;
            }
        }

        private static async Task RunTaskAsync(Type type, IServiceScope scope, MethodInfo method)
        {
            var task = scope.ServiceProvider.GetRequiredService(type);
            var isAsync = method.ReturnType == typeof(Task);
            if (isAsync)
            {
                await (Task)method.Invoke(task, null);
            }
            else
            {
                method.Invoke(task, null);
            }
        }
    }
}
