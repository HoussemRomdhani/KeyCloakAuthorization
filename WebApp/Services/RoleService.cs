using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services;

public class RoleService
{
    private readonly RoleRepository roleRepository;
    public RoleService(RoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public void TryAddRoles(IList<string> roles)
    {
        foreach (var role in roles)
        {
            TryAddRole(role);
        }
    }

    private void TryAddRole(string roleName)
    {
        Role? role = roleRepository.GetRoleByName(roleName);

        if (role == null)
        {
            roleRepository.AddRole(roleName);
        }
    }

    public List<Role> GetRoles()
    {
        return roleRepository.GetRoles();
    }
}

