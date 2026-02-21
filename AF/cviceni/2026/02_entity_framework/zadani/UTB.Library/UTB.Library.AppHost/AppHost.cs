using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres;

if (builder.Environment.IsEnvironment("Testing"))
{
    postgres = builder.AddPostgres("postgres-testing");
}
else
{
    postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin(c =>
                      {
                          c.WithLifetime(ContainerLifetime.Persistent);
                          c.WithImage("dpage/pgadmin4:latest");
                      })
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.AddProject<Projects.UTB_Library_WebApi>("webapi")
       .WithReference(database)
       .WaitFor(database);

builder.Build().Run();
