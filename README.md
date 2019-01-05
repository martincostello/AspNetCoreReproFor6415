# ASP.NET Core Repro For Issue 6415

Repro for https://github.com/aspnet/AspNetCore/issues/6415.

## Instructions

To reproduce the issue:

  1. Clone this repository
  1. Open `AspNetCoreReproFor6415.sln`
  1. Run the `AspNetCoreReproFor6415` project in the debugger with `F5` (or just run) using IIS Express
  1. With the ASP.NET Core application running, run the `AspNetCoreReproFor6415.Client` console application

Within a few minutes, the client application should print an exception caused by an HTTP 500 response from the MVC application to the console.

Running the client application with Refit [request buffering enabled](https://github.com/reactiveui/refit#buffering-and-the-content-length-header) using a command such as `dotnet run --configuration Release -- --buffered` will not reproduce the error.

Changing the [`ancmHostingModel`](https://github.com/martincostello/AspNetCoreReproFor6415/blob/386b55ba08925a2163f0020bf22307d6edd45a2d/AspNetCoreReproFor6415/Properties/launchSettings.json#L14) property to `OutOfProcess` and running the MVC application will also not reproduce the error.
