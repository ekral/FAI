using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing-UTB.MinuteMeal");   
}
else
{
    postgres = builder.AddPostgres("postgres")
                     .WithContainerName("postgres-UTB.MinuteMeal")
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);                     
}

var database = postgres.AddDatabase("database");

var webapi = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
                        .WithReference(database)
                        .WaitFor(database);

builder.Build().Run();
