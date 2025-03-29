using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaYarp();

builder.Services.AddBff(options =>  options.ManagementBasePath = "/account")
    .AddServerSideSessions()
    .AddRemoteApis();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    o.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(o =>
    {
        o.Cookie.Name = "__NG-CLIENT-BFF";
        o.Cookie.SameSite = SameSiteMode.Strict;
        o.Events.OnRedirectToLogin = (context) =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = "http://localhost:8080/realms/demo";
        options.ClientId = "ng-client-bff";
        options.ClientSecret = "RoP7XCbkdbAhABwU3PhQjS8roZPaAQm6";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();

app.UseAuthentication();

app.UseBff();

app.MapBffManagementEndpoints();

app.MapRemoteBffApiEndpoint("/api", "http://localhost:5157/api").RequireAccessToken();

app.UseSpaYarp();

app.Run();
