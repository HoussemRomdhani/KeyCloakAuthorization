using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserService userService;
    private readonly RoleService roleService;

    public AuthController(UserService userService, RoleService roleService)
    {
        this.userService = userService;
        this.roleService = roleService;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        RegisterViewModel vm = new()
        {
            Roles = roleService.GetRoles().Select(role => new SelectListItem()
            {
                Value = role.Id.ToString(),
                Text = role.RoleName
            }).ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult UserInfo()
    {
        if ((User.FindFirst(ClaimTypes.NameIdentifier)?.Value) == null ||
           !int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized();
        }


        var user = userService.GetUserById(userId);

        if (user == null)
        {
            return NotFound();
        }

        var model = new UpdateUserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isValid = userService.ValidateUser(model.Username, model.Password);

        if (!isValid)
        {
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        User? user = userService.GetUserByUserame(model.Username);

        if (user == null)
        {
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        var claims = GetUserClaims(user);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

       await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); 
        }

       User? user =  userService.GetUserByUserame(model.Username);

        if (user != null)
        {
            ViewBag.Error = "Username already taken.";
            return View();
        }

        if (userService.RegisterUser(model.Username, model.Password, model.FirstName, model.LastName))
        {
            ViewBag.Message = "Registration successful! You can now log in.";
        }

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> UserInfo(UpdateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = userService.GetUserById(model.Id);

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = model.FirstName;
      
        user.LastName = model.LastName;

       bool success = userService.UpdateUser(user);

        if (success)
        {
            var claims = GetUserClaims(user);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        return Redirect("/Home/Index");
    }

    private List<Claim> GetUserClaims(User user)
    {
        IList<Role> userRoles = userService.GetRolesByUserId(user.Id);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        }

        return claims;
    }
}
