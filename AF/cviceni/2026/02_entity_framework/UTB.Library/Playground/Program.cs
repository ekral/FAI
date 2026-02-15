using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UTB.Library.Db;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.AddNpgsqlDbContext<LibraryContext>("database");

var host = builder.Build();

var context = host.Services.GetService<LibraryContext>();

foreach(Author author in context.Authors)
{
    Console.WriteLine(author.Name);
}
