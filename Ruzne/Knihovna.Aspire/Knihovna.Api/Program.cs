using Knihovna.Data;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        // builder.AddSqlServerClient(connectionName: "knihovna-database");

        builder.Services.AddAuthorization();
        builder.AddSqlServerDbContext<KnihovnaDbContext>(connectionName: "knihovna-database");
        //builder.Services.AddDbContextPool<KnihovnaDbContext>(b => b.UseSqlServer(builder.Configuration.GetConnectionString("knihovna-database")));
       
        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/knihovna", (KnihovnaDbContext context) =>
        {
            return context.Autori.FirstOrDefault();
        });

        app.Run();
    }
}
