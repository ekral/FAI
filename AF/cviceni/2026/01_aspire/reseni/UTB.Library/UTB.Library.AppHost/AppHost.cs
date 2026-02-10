var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("database");

builder.AddProject<Projects.UTB_Library_DbManager>("utb-library-dbmanager")
       .WithReference(database)
       .WithHttpCommand("reset-db", "Reset Database")
       .WaitFor(database);

builder.Build().Run();
