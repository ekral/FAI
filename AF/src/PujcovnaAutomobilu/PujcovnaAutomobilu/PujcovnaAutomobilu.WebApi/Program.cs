using PujcovnaAutomobilu.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PujcovnaAutomobiluContext>();

var app = builder.Build();

app.MapGet("/", (PujcovnaAutomobiluContext context) => context.Automobils);

app.Run();
