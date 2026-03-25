using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;
IResourceBuilder<PostgresDatabaseResource> database;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing-UTB.School");

    database = postgres.AddDatabase("database");
}
else
{
    postgres = builder.AddPostgres("postgres")
                     .WithContainerName("postgres-UTB.School")
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);

    database = postgres.AddDatabase("database");

    builder.AddProject<Projects.UTB_School_DatabaseManager>("dbmanager")
           .WithReference(database)
           .WithHttpCommand("/dev/seed", "Reset Database")
           .WaitFor(database);
}

var webapi = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
                        .WithReference(database)
                        .WaitFor(database);

builder.Build().Run();
