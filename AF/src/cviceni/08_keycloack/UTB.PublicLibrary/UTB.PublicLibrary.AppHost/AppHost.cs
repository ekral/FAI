using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


if (builder.Environment.IsEnvironment("Testing"))
{
    var postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("utb-publiclibrary-postgres-testing");

    var database = postgres.AddDatabase("database");

    var keycloak = builder.AddKeycloak("keycloak", 8080)
                          .WithRealmImport("import")
                          .WithHttpsEndpoint(port: 8443, name: "https")
                          .WithContainerName("utb-publiclibrary-keycloak-testing");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                                   .WithReference(database)
                                   .WithReference(keycloak)
                                   .WaitFor(database)
                                   .WaitFor(keycloak);
}
else
{
    var postgres = builder.AddPostgres("postgres")
                      .WithContainerName("utb-publiclibrary-postgres")
                      .WithDataVolume("utb-publiclibrary-postgres-data")
                      .WithLifetime(ContainerLifetime.Persistent);

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_PublicLibrary_DatabaseManager>("databasemanager")
                               .WithReference(database)
                               .WithHttpCommand("/dev/seed", "Restart Database")
                               .WaitFor(database);

    var keycloak = builder.AddKeycloak("keycloak", 8080)
                          .WithRealmImport("import")
                          .WithHttpsEndpoint(port: 8443, name: "https")
                          .WithContainerName("utb-publiclibrary-keycloak")
                          .WithDataVolume("utb-publiclibrary-keycloak-data")
                          .WithLifetime(ContainerLifetime.Persistent);

    var webapi = builder.AddProject<Projects.UTB_PublicLibrary_WebApi>("webapi")
                        .WithReference(database)
                        .WithReference(keycloak)
                        .WaitFor(database)
                        .WaitFor(keycloak);

    _ = builder.AddProject<Projects.UTB_PublicLibrary_Web>("web")
                                   .WithReference(webapi)
                                   .WithReference(keycloak)
                                   .WaitFor(webapi)
                                   .WaitFor(keycloak);

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebSse>("websse")
                                   .WithReference(webapi)
                                   .WaitFor(webapi);

    _ = builder.AddProject<Projects.UTB_PublicLibrary_WebSseJavascript>("webssejavascript")
                                   .WithReference(webapi)
                                   .WaitFor(webapi);
}

builder.Build().Run();