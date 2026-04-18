var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.UseDefaultFiles(); // Automaticky hledá index.html
app.UseStaticFiles();  // Povolí obsluhu statických souborů (wwwroot)

app.Run();