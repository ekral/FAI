using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication("x").AddIdentityCookies();
builder.Services.AddAuthorization();
builder.Services.AddCors();

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddDbContext<IdentityDbContext>(o => o.UseInMemoryDatabase("myDb"));

builder.Services.AddOpenApi();

var app = builder.Build();


//app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors(p => p.WithOrigins("https://localhost:7121").AllowCredentials().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();



app.MapGroup("/account").MapIdentityApi<IdentityUser>();


app.MapGet("/data", () => "joo").RequireAuthorization();

app.Run();
