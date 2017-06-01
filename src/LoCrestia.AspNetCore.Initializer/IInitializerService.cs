using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public interface IInitializerService
    {

        Task InitAsync();

        InitializerResult Result { get; }

    }
}
