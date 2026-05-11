var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin(pg => pg.WithContainerName("utb-concurrencytest-pgadmin"))
                      .WithContainerName("utb-concurrencytest-postgres")
                      .WithDataVolume("utb-concurrencytest-postgres-data")
                      .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("database");

var webapi = builder.AddProject<Projects.UTB_ConcurrencyTest_WebApi>("webapi")
                    .WithHttpCommand("/dev/seed", "Seed")
                    .WithHttpCommand("/dev/test", "Test")
                    .WithReference(database)
                    .WaitFor(database);

builder.Build().Run();
