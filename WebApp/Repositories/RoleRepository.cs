using Microsoft.Data.SqlClient;
using WebApp.Models;

namespace WebApp.Repositories;

public class RoleRepository
{
    private readonly string connectionString;

    public RoleRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public bool AssignRoleToUser(int userId, int roleId)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool AddRole(string role)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "INSERT INTO Roles (RoleName) VALUES (@RoleName)";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RoleName", role);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool UnassignRoleToUser(int userId, int roleId)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public List<Role> GetRolesByUserId(int userId)
    {
        var roles = new List<Role>();
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "SELECT r.Id, r.RoleName FROM Roles r INNER JOIN UserRoles ur ON r.Id = ur.RoleId WHERE ur.UserId = @UserId";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }
        }
        return roles;
    }

    public List<Role> GetRolesNotAssignedToUser(int userId)
    {
        var roles = new List<Role>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT r.Id, r.RoleName
                      FROM Roles r
                      LEFT JOIN UserRoles ur ON r.Id = ur.RoleId AND ur.UserId = @UserId
                      WHERE ur.RoleId IS NULL";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }
        }
        return roles;
    }

    public List<Role> GetRoles()
    {
        var roles = new List<Role>();
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "SELECT Id, RoleName FROM Roles";
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role
                        {
                            Id = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }
        }
        return roles;
    }

    public Role? GetRoleByName(string roleName)
    {
        Role? role = null;
        using (var conn = new SqlConnection(connectionString))
        {
            var query = "SELECT Id, RoleName FROM Roles WHERE RoleName = @RoleName";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        role =  new Role
                        {
                            Id = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        };
                    }
                }
            }
        }

        return role;
    }
}
