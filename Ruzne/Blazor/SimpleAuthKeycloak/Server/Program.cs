using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//docker run -p 8080:8080 - e KC_BOOTSTRAP_ADMIN_USERNAME = admin - e KC_BOOTSTRAP_ADMIN_PASSWORD = admin quay.io / keycloak / keycloak:26.1.3 start - dev

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
