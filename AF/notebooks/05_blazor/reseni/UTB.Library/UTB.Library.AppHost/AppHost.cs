using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing")
                      .WithContainerName("UTB.Library-postgres-testing");

}
else
{
    postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin(c =>
                      {
                          c.WithLifetime(ContainerLifetime.Persistent)
                           .WithImage("dpage/pgadmin4:latest")
                           .WithContainerName("UTB.Library-pgadmin");
                      })
                      .WithContainerName("UTB.Library-postgres")
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

var webapi = builder.AddProject<Projects.UTB_Library_WebApi>("webapi")
       .WithReference(database)
       .WaitFor(database);

builder.AddProject<Projects.UTB_Library_Web>("web")
       .WithReference(webapi)
       .WaitFor(webapi);

builder.Build().Run();
