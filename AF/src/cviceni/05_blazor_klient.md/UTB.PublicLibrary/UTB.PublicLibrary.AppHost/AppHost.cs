using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing-UTB.PublicLibrary");
}
else
{
    postgres = builder.AddPostgres("postgres")
                      .WithContainerName("postgres-UTB.PublicLibrary")
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("database");

var webapi = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                    .WithReference(database)
                    .WaitFor(database);

builder.Build().Run();