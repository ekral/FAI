using Menza.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MenzaContext>();

var app = builder.Build();

app.MapGet("/", (MenzaContext context) => context.Jidla);

// 🚀 Get ktery vrati jidlo podle Id

app.Run();
