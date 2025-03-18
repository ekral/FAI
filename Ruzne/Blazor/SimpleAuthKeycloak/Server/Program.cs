using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(c =>
{
    c.MetadataAddress = "http://localhost:8080/realms/my-realm/.well-known/openid-configuration";
    c.Authority = "http://localhost:8080/realms/my-realm";
    c.Audience = "account";
    c.RequireHttpsMetadata = false;
});


builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors(p => p.WithOrigins("https://localhost:7121").AllowCredentials().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/data", () => "joo").RequireAuthorization();

app.Run();
