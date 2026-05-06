using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    var postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("utb-school-postgres-testing");

    var database = postgres.AddDatabase("database");

    var keycloak = builder.AddKeycloak("keycloak", 8080)
           .WithRealmImport("./Realm")
           .WithContainerName("utb-school-keycloak-testing");

    _ = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
               .WithReference(database)
               .WithReference(keycloak)
               .WaitFor(database)
               .WaitFor(keycloak);
}
else
{
    var postgres = builder.AddPostgres("postgres")
                     .WithContainerName("utb-school-postgres")
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_School_DatabaseManager>("dbmanager")
               .WithReference(database)
               .WithHttpCommand("/dev/seed", "Reset Database")
               .WaitFor(database);

    var keycloak = builder.AddKeycloak("keycloak", 8080)
               //.WithRealmImport("./Realm")
               .WithContainerName("utb-school-keycloak")
               .WithDataVolume("utb-school-keycloak-data")
               .WithLifetime(ContainerLifetime.Persistent);

    var webapi = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
                        .WithReference(keycloak)
                        .WithReference(database)
                        .WaitFor(database)
                        .WaitFor(keycloak);

    _ = builder.AddProject<Projects.UTB_School_Web>("web")
               .WithReference(keycloak)
               .WithReference(webapi)
               .WaitFor(webapi);

    _ = builder.AddProject<Projects.UTB_School_WebSse>("websse")
           .WithReference(keycloak)
           .WithReference(webapi)
           .WaitFor(webapi);

    _ = builder.AddProject<Projects.UTB_School_WebSseJavascript>("webssejavascript")
       .WithReference(webapi)
       .WaitFor(webapi);
}

builder.Build().Run();
