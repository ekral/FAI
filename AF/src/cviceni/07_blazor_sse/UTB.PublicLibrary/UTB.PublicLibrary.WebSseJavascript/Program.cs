var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseDefaultFiles(); // Automaticky hledá index.html
app.UseStaticFiles();  // Povolí obsluhu statických souborů (wwwroot)

app.Run();