# Core-Init (one ASP.NET Core initializer)

> **WE'VE MOVED TO NETCORE2!** :) Version 0.3.0 is a `netcoreapp2.0` 

Sometimes in your ASP.NET Core projects you need to create some initialization steps (i. e. seed a DbContext). Usually this is done in the `Startup` class in a code like that:

```
MyDbContextSeeder.SeedAsync(dbContext).Wait();
```

This code works, but it has so many drawbacks:

1. Your app don't start to process requests until the code of the `SeedAsync` method finishes
2. Code of `Startup` class is executed when using some cli tools like `dotnet ef migrations`. This is not desired (i.e. you don't want to seed your DbContext every time you create a migration)

Core-Init wants to help with this.

# How to contribute

Just submit an issue or (even better!) a pull request. Please **if you want to submit a PR, submit from the dev branch, not from master!**

## How to build

Just clone the repo (dev or master branch) and build the `LoCrestia.AspNetCore.Initializer.sln` VS2017 solution. Solution contains two projects (core-init itself and a web project which is just a demo)

You can also use `dotnet` cli if you prefer.

# How to use

Just add the `LoCrestia.AspNetCore.Initializer` package to your project (current version **0.3.0**)

```
Install-Package LoCrestia.AspNetCore.Initializer
```

#0.3.0 CHANGELOG

- **breaking change**: Removed `IApplicationBuilder` extension method `RunInitializationsAsync()` used to launch the initialization tasks.
- **breaking change**: The initialization tasks are launched using extension method `RunInitTasks` from `IWebHost`.
- Support for tasks defined either in `Startup` class or in the `Main` method.
- Support for tasks that use scoped objects.

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

You can also add initialization steps in the `Main` method, using the extension method `RunInitTasks`:

```
public static void Main(string[] args)
{
    BuildWebHost(args)
        .RunInitTasks(opt =>
        {
            opt.AddTask<MyContext>("EF Seed", async (ctx) =>
            {
                ctx.Database.Migrate();
                for (var i = 0; i < 1000; i++)
                {
                    ctx.MyEntities.Add(new MyEntity() { Name = $"Test {i}" });
                }
                await ctx.SaveChangesAsync();
            });
        })
        .Run();
}
```

## Initialization steps types. Functions

Currently two supported types of initialization steps are supported: method-based and class-based.

The first ones are added using the `AddTask` and passing a `Func<Task>` which contains the code to run:

```
options.AddTask(async () => Task.Delay(10000));
```

If you use the `RunInitTasks` method you can use:

* `AddTask<T>`: Which adds a Task that is a function that takes a parameter of type `T`. This parameter is resolved via DI. This method takes a `Func<T, Task>` as a parameter that contains the init task code.
* `AddTask`: Which adds a Task that is a function that do not take parameters. This method takes a `Func<Task>` as a parameter that contains the init task code.

There is no (currently) support for class-based tasks using `RunInitTasks`.

## Initialization steps types. Class-based

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

The extension method `RunInitTasks` of the `IWebHost` runs ALL initializations tasks.

```
BuildWebHost(args)
        .RunInitTasks()
        .Run();
```

If you added any initialization task using the `RunInitTasks` that accepts one parameter, you **don't need to call it again**. But if you added only tasks using `Startup` class need to call this version with no parameters.

> **RunInitTasks must be called once** with parameters (for also adding tasks) or without parameters. Don't call it multiple times!

### Handling errors

If an exception happens during the execution of a initialization task, core-init allows two options:

1. Continue with the next initialization task
2. Stop the initialization process, but don't rethrow the exception. All pending tasks are not run.

If you use `ContinueOnError()` when adding a task, core-init will run next task if an exception happens:

```
options.AddTask(async () => throw new Exception("foo")).ContinueOnError();
```

If you don't use `ContinueOnError()` and the initialization task has an exception, the initialization process stops.

### Viewing the result of initialization

When registering core-ini (in `Main` method) you can enter a URL that will return a JSON with the status of the initialization process:

1. If Started or not
2. Task running

Once finished it will show the status of all tasks (run correctly, run with error or skipped). If there is an exception the exception details are also serialized.

To set the endpoint use the property `ResultPath` of the options:

```
.UseInitializer(options =>
{
    options.ResultPath = "/initresult";
})
```

If you set `ResultPath` to `null` the endpoint is disabled.

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