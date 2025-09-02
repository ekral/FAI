namespace WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            
            builder.Services.AddAuthentication().AddJwtBearer(c =>
            {
                c.Authority = "http://127.0.0.1:8080/realms/mujrealm";
                c.Audience = "account";
                c.RequireHttpsMetadata = false;
            });

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseCors(p => p.WithOrigins("https://localhost:7221").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGet("/data", () => "joo").RequireAuthorization();

            app.Run();
        }
    }
}
