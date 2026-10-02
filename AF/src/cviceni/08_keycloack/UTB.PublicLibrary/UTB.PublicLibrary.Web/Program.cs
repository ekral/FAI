using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UTB.PublicLibrary.Web;
using UTB.PublicLibrary.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddKeycloakOpenIdConnect(
    serviceName: "keycloak",
    realm: "utb-publiclibrary",
    options =>
    {
        options.ClientId = "utb-publiclibrary-web";
        options.ClientSecret = "qDW7aoS5LVNmQNqA6oTHNyBRp5Ahsdge";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("openid"); // id_token
        options.Scope.Add("offline_access"); // refresh_token
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false; // jen dev
        options.TokenValidationParameters.NameClaimType = "preferred_username";
    }
);

builder.Services.AddOpenIdConnectAccessTokenManagement(options =>
    options.RefreshBeforeExpiration = TimeSpan.FromSeconds(30)
);

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddUserAccessTokenHttpClient<LibraryService>(
    configureClient: (_, client) => client.BaseAddress = new Uri("https://webapi"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    //.RequireAuthorization(pb => pb.RequireRole("books-admin")) // autorizace cele aplikace
    .AddInteractiveServerRenderMode();

app.MapGet("/login", async (HttpContext ctx, string? returnUrl) =>
{
    string redirectUri = "/";

    if (!string.IsNullOrWhiteSpace(returnUrl) && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
    {
        redirectUri = returnUrl;
    }

    await ctx.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = redirectUri,
        IsPersistent = false
    });
});

// Logout dělám přes form a post kvůli dvojitému načítání stránky
app.MapPost("/logout", async (HttpContext ctx) =>
{
    string? idToken = await ctx.GetTokenAsync("id_token");

    await ctx.RevokeRefreshTokenAsync();

    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await ctx.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/books",
        Parameters = { { "id_token_hint", idToken ?? string.Empty } }
    });
});

app.Run();

