using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("utb-publiclibrary-postgres-testing");

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                    .WithReference(database)
                    .WaitFor(database);
}
else
{
    postgres = builder.AddPostgres("postgres")
                      .WithContainerName("utb-publiclibrary-postgres")
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                    .WithReference(database)
                    .WaitFor(database);

    _ = builder.AddProject<Projects.UTB_PublicLibrary_DatabaseManager>("databasemanager")
                               .WithReference(database)
                               .WithHttpCommand("/dev/seed", "Restart Database")
                               .WaitFor(database);
}

builder.Build().Run();



