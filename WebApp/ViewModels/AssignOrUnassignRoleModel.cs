using WebApp.Models;

namespace WebApp.ViewModels;

public class AssignOrUnassignRoleModel
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public List<Role> Roles { get; set; } = [];
}
