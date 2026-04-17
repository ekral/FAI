using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Testing"))
{
    var postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("postgres-testing-UTB.School");

    var database = postgres.AddDatabase("database");

    _ = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
               .WithReference(database)
               .WaitFor(database);
}
else
{
    var postgres = builder.AddPostgres("postgres")
                     .WithContainerName("postgres-UTB.School")
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);

    var database = postgres.AddDatabase("database");

    builder.AddProject<Projects.UTB_School_DatabaseManager>("dbmanager")
           .WithReference(database)
           .WithHttpCommand("/dev/seed", "Reset Database")
           .WaitFor(database);

    var webapi = builder.AddProject<Projects.UTB_School_WebApi>("webapi")
                        .WithReference(database)
                        .WaitFor(database);

    _ = builder.AddProject<Projects.UTB_School_Web>("web")
               .WithReference(webapi)
               .WaitFor(webapi);

    _ = builder.AddProject<Projects.UTB_School_WebSse>("websse")
           .WithReference(webapi)
           .WaitFor(webapi);

    _ = builder.AddProject<Projects.UTB_School_WebSseJavascript>("webssejavascript")
       .WithReference(webapi)
       .WaitFor(webapi);

}




builder.Build().Run();
