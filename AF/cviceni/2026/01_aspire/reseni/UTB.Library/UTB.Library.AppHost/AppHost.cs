var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("mojesql")
                 .WithDataVolume()
                 .WithLifetime(ContainerLifetime.Persistent);

var database = sql.AddDatabase("mojedatabase");

builder.Build().Run();
