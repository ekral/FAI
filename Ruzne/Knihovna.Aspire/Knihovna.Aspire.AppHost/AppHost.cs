using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
       .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("knihovna-database");

var seeder = builder.AddProject<Knihovna_Seeder>("seeder")
        .WithReference(db)
        .WaitFor(db);

builder.AddProject<Projects.Knihovna_Api>("knihovna-api")
       .WithReference(db)
       .WaitFor(db)
       .WaitForCompletion(seeder);


builder.Build().Run();
