using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("database");

using var host = builder.Build();

// var configuration = host.Services.GetRequiredService<IConfiguration>();
// var connectionString = configuration.GetConnectionString("appdb");

var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Started");
});
lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("Stopping firing");
    Console.WriteLine("Stopping end");
});
lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("Stopped firing");
    Console.WriteLine("Stopped end");
});

host.Start();

// Listens for Ctrl+C.
host.WaitForShutdown();


