# Core-Init (one ASP.NET Core initializer)

Sometimes in your ASP.NET Core projects you need to create some initialization steps (i. e. seed a DbContext). Usually this is done in the `Startup` class in a code like that:

```
MyDbContextSeeder.SeedAsync(dbContext).Wait();
```

This code works, but blocks all requests until the `SeedAsync` method finishes. Core-Init tries to help in those cases.

# How to use

Just add the `LoCrestia.AspNetCore.Initializer` package to your project (current version 0.1.0)

```
Install-Package LoCrestia.AspNetCore.Initializer
```

# How to contribute

Just submit an issue or (even better!) a pull request. Please **if you want to submit a PR, submit from the dev branch, not from master!**

## How to build

Just clone the repo (dev or master branch) and build the `LoCrestia.AspNetCore.Initializer.sln` VS2017 solution. Solution contains two projects (core-init itself and a web project which is just a demo)

You can also use `dotnet cli` if you prefer.

## Adding Core-Init

You add Core-Init to your project in the `Main` method (found usually in `Program.cs`):

```
var host = new WebHostBuilder()
    .UseKestrel()
    .UseInitializer(options =>
        {
            options.AddTask(async () => await Task.Delay(10000));
        } 
    )
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration()
    .UseStartup<Startup>()
    .UseApplicationInsights()
    .Build();
```

The method `UseInitializer` adds Core-Init to the pipeline of the application and creates all needed infrastructure. You can add a initialization steps here (using `AddTask`) but is not required.

## Adding initialization steps

As said before, one place when you can add initialization steps is in the `Main` method, but this is sometimes not useful (because you have not access to ASP.NET Core objects). You can add also initialization steps in the `Configure` method of the `Startup` class:

```
app.RunInitializationsAsync(options =>
{
    options.AddTask(async () => await Task.Delay(10000));
}).ConfigureAwait(false);
```

The method `RunInitializationsAsync` runs ALL initializations tasks (the ones you included in the `UseInitializer` in the `Main` method and the ones added in the `RunInitializationsAsync`).

**Note:** You should not wait for the `RunInitializationsAsync` method to finish!

## Init Middleware

While your initializations tasks are running your application can start receiving requests: all requests will get a 503 (Service Unavailable) until all initialization tasks are completed. Once all are completed requests will be processed in a normal way.

This allows you run initialization tasks without blocking requests.

### Changing initialization response

By default all requests sent while initialization tasks are running receive a 503 with the content-type "text/plain" and the text "Service is Starting...". You can change the Status Code and the text (but not the content-type yet) in the `RunInitializationsAsync`, using the properties `ErrorText` and `StatusCode`:

```
app.RunInitializationsAsync(options =>
{
    options.ErrorText = "Wait a moment... we are doing some needed stuff";
}).ConfigureAwait(false);
```

# Roadmap

1. More customization in responses while running initialization tasks
2. Cancelling initialization tasks
3. ...

**This is a preliminary version! Expect some breaking changes in following versions**