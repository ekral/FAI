var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                 .WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent))
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.Build().Run();
