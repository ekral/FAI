var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("utb-library-dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.AddProject<Projects.Playground>("playground")
       .WithReference(database)
       .WaitFor(database);

builder.Build().Run();
