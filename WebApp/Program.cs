using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using WebApp.Authorization;
using WebApp.Constants;
using WebApp.Logging;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders(); 
        builder.Logging.AddConsole();

        string connectionstring = builder.Configuration.GetConnectionString("DefaultConnection")!;

        builder.Services.AddSingleton(new RoleRepository(connectionstring));
        builder.Services.AddSingleton(new UserRepository(connectionstring));

        builder.Services.AddSingleton(new BookRepository(connectionstring));
        builder.Services.AddSingleton<RoleService>();
        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<BookService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IAuthorizationHandler, MustOwnBookHandler>();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });

        builder.Services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.AddPolicy("MustOwnBook", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddRequirements(new MustOwnBookRequirement());
                });
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();

        app.UseRouting();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        using (var scope = app.Services.CreateScope())
        {
            var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();

            roleService.TryAddRoles([Roles.ADMIN, Roles.Author, Roles.User]);

            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            userService.RegisterAdmin("admin", "admin");
        }
        app.Run();
    }
}

