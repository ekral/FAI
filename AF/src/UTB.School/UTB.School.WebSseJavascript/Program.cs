var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Povolí obsluhu statických souborů (wwwroot)
app.UseDefaultFiles(); // automaticky hledá index.html
app.UseStaticFiles();

app.Run();