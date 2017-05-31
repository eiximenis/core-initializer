# Core-Init (one ASP.NET Core initializer)

Sometimes in your ASP.NET Core projects you need to create some initialization steps (i. e. seed a DbContext). Usually this is done in the `Startup` class in a code like that:

```
MyDbContextSeeder.SeedAsync(dbContext).Wait();
```

This code works, but blocks all requests until the `SeedAsync` method finishes. Core-Init tries to help in those cases.

# How to contribute

Just submit an issue or (even better!) a pull request. Please **if you want to submit a PR, submit from the dev branch, not from master!**

## How to build

Just clone the repo (dev or master branch) and build the `LoCrestia.AspNetCore.Initializer.sln` VS2017 solution. Solution contains two projects (core-init itself and a web project which is just a demo)

You can also use `dotnet` cli if you prefer.

# How to use

Just add the `LoCrestia.AspNetCore.Initializer` package to your project (current version 0.1.0)

```
Install-Package LoCrestia.AspNetCore.Initializer
```

## Adding Core-Init

You add Core-Init to your project in the `Main` method (found usually in `Program.cs`) when creating the `WebHostBuilder` object:

```
var host = new WebHostBuilder()
    .UseKestrel()
    .UseInitializer()
```

The method `UseInitializer` adds Core-Init to the pipeline of the application and creates all needed infrastructure.

## Adding initialization steps

You can add initialization steps in the `ConfigureServices` method of the `Startup` class, using the extension method `AddInitTasks`:

```
services.AddInitTasks(options =>
{
    options.AddTask(async () => Task.Delay(10000));
});
```

## Initialization steps types

Currently two supported types of initialization steps are supported: method-based and class-based.

The first ones are added using the `AddTask` and passing a `Func<Task>` which contains the code to run:

```
options.AddTask(async () => Task.Delay(10000));
```

But you can create your own complex initialization steps by simply having one class with a method called `Run`:

```
public class MyCustomTask
{
    private readonly ILogger _logger;
    public MyCustomTask(ILogger<MyCustomTask> logger)
    {
        _logger = logger;
    }

    public async Task Run()
    {
        await Task.Delay(10000);
    }
}
```

Note that the class do not need to inherit from any other class, nor implement any interface. Only the `Run` method is required. Also note that you can inject any other required service in the constructor.

The return value of mtehod Run is ignored **but the method `Run` can return a `Task` to indicate that is an asynchronous method**. If the method `Run` returns a Task, it will be called using `await`. Only `Task` is supported (`Task<T>` and any other awaitable object is not supported).

Once you have the class created you simply call the generic version of the `AddTask` method with no parameters:

```
options.AddTask<MyCustomTask>();
```

For this to work you must register the task class (`MyCustomTask`) to the DI system (in the `ConfigureServices` method)

```
services.AddTransient<MyCustomTask>();
```

## Running the initialization steps

The extension method `RunInitializationsAsync` of the `IApplicationBuilder` runs ALL initializations tasks. This method do not accept parameters: 

```
app.RunInitializationsAsync().ConfigureAwait(false);
```

Usually this method is called at the end of `Configure` method of the `Startup` class.

**Note:** You should not wait for the `RunInitializationsAsync` method to finish!

## Init Middleware

While your initializations tasks are running your application can start receiving requests: all requests will get a 503 (Service Unavailable) until all initialization tasks are completed. Once all are completed requests will be processed in a normal way.

This allows to start your server application fast and start to serving requests (ok, they will receive a 503 but in a future this behavior will be more customizable).

### Changing initialization response

By default all requests sent while initialization tasks are running receive a 503 with the content-type "text/plain" and the text "Service is Starting...". You can change the Status Code and the text (but not the content-type yet) in the `UseInitializer`, using the properties `ErrorText` and `StatusCode`:

```
.UseInitializer(options =>
{
    options.ErrorText = "Doing some stuff";
    options.StatusCode = 500;
})
```

# Roadmap

1. More customization in responses while running initialization tasks
2. Cancelling initialization tasks
3. ...

**This is a preliminary version! Expect some breaking changes in following versions**