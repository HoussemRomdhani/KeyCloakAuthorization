using WebApp.Constants;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services;

public class UserService
{
    private readonly UserRepository userRepository;

    private readonly RoleRepository roleRepository;

    public UserService(UserRepository userRepository, RoleRepository roleRepository)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
    }

    public List<User> GetAllUsers()
    {
        return userRepository.GetAllUsers();
    }
  
    public User? GetUserById(int id)
    {
        return userRepository.GetUserById(id);
    }

    public User? GetUserByUserame(string username)
    {
        return userRepository.GetUserByUserame(username);
    }

    public bool ValidateUser(string username, string password)
    {
        User? user = userRepository.GetUserByUsername(username);

        return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public bool RegisterUser(string username, string password, string firstName, string lastName)
    {
        var existingUser = userRepository.GetUserByUsername(username);
     
        if (existingUser != null)
        {
            return false;
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        User? user = new()
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = hashedPassword
        };
       
        bool success =  userRepository.AddUser(user);

        if (!success)
        {
            return false;
        }

         user = userRepository.GetUserByUsername(username);

        if (user == null)
        {
            return false;
        }

        Role? role = roleRepository.GetRoleByName(Roles.User);

        if (role == null)
        {
            return false;
        }

        return AssignRoleToUser(user.Id, role.Id);
    }

    public bool RegisterAdmin(string username, string password)
    {
        var existingUser = userRepository.GetUserByUsername(username);

        if (existingUser != null)
        {
            return false;
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        bool success = userRepository.AddUser(new User { Username = username, FirstName = username, LastName = username, PasswordHash = hashedPassword });

        if (!success)
        {
            return false;
        }

        var user = userRepository.GetUserByUsername(username);

        if (user == null)
        {
            return false;
        }

        Role? role = roleRepository.GetRoleByName(Roles.ADMIN);

        if (role == null)
        {
            return false;
        }

        return AssignRoleToUser(user.Id, role.Id);
    }

    public bool AuthenticateUser(string username, string password)
    {
        var user = userRepository.GetUserByUsername(username);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public List<Role> GetRolesByUserId(int userId)
    {
        return roleRepository.GetRolesByUserId(userId);
    }

    public List<Role> GetRolesNotAssignedToUser(int userId)
    {
        return roleRepository.GetRolesNotAssignedToUser(userId);
    }

    public bool AssignRoleToUser(int userId, int roleId)
    {
        return roleRepository.AssignRoleToUser(userId, roleId);
    }

    public bool UnassignRoleToUser(int userId, int roleId)
    {
        return roleRepository.UnassignRoleToUser(userId, roleId);
    }
  
    public bool UpdateUser(User user)
    {
        return userRepository.UpdateUser(user);
    }
}

