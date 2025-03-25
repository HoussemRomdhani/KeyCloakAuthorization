using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Constants;
using WebApp.Services;
using WebApp.ViewModels;

[Authorize(Roles =(Roles.ADMIN))]
public class UserController : Controller
{
	private readonly UserService userService;
	private readonly RoleService roleService;

	public UserController(UserService userService, RoleService roleService)
	{
		this.userService = userService;
		this.roleService = roleService;
	}

	public IActionResult Index()
	{
		IList<UserWithRolesViewModel> vm = [];

		var users = userService.GetAllUsers().Where(user => !user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase)).ToList();

		var allRoles = roleService.GetRoles();

		foreach (var user in users)
		{
			var userRoles = userService.GetRolesByUserId(user.Id);

			var roles = string.Join(",", userRoles.Select(role => role.RoleName).ToList());

			vm.Add(new UserWithRolesViewModel
			{
				Id = user.Id,
				Username = user.Username,
				Roles = roles,
				CanAssignRole = userRoles.Count < allRoles.Count,
				CanUnAssignRole = userRoles.Count > 0
			});
		}

		return View(vm);
	}

	public IActionResult AssignRole(int id)
	{
		var user = userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

		var roles = userService.GetRolesNotAssignedToUser(id);

		AssignOrUnassignRoleModel assignRoleModel = new()
		{
			UserId = user.Id,
			Roles = roles
		};

		return View(assignRoleModel);
	}

	public IActionResult UnassignRole(int id)
	{
		var user = userService.GetUserById(id);

		if (user == null)
		{
			return NotFound();
		}

		var roles = userService.GetRolesByUserId(id);

		AssignOrUnassignRoleModel assignRoleModel = new()
		{
			UserId = user.Id,
			Roles = roles
		};

		return View(assignRoleModel);
	}

	[HttpPost]
	public IActionResult AssignRole(int userId, int roleId)
	{
		var success = userService.AssignRoleToUser(userId, roleId);
		
		if (!success)
        {
			ViewBag.Error = "Role assignment failed.";
			return View();
        }

        return RedirectToAction(nameof(Index));

	}

	[HttpPost]
	public IActionResult UnassignRole(int userId, int roleId)
	{
		var success = userService.UnassignRoleToUser(userId, roleId);
		if (!success)
        {
			ViewBag.Error = "Role unassignment failed.";
			return View();
        }
		return RedirectToAction(nameof(Index));
	}

  
}
