using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


if (builder.Environment.IsEnvironment("Testing"))
{
    var postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("utb-publiclibrary-postgres-testing");

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                                   .WithReference(database)
                                   .WaitFor(database);
}
else
{
    var postgres = builder.AddPostgres("postgres")
                      .WithContainerName("utb-publiclibrary-postgres")
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_DatabaseManager>("databasemanager")
               .WithReference(database)
               .WithHttpCommand("/dev/seed", "Restart Database")
               .WaitFor(database);

    var webapi = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                        .WithReference(database)
                        .WaitFor(database);

    _ = builder.AddProject<Projects.UTB_PublicLibrary_Web>("web")
               .WithReference(webapi)
               .WaitFor(webapi);
}

builder.Build().Run();