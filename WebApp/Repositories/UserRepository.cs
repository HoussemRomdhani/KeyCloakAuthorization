using Microsoft.Data.SqlClient;
using WebApp.Models;

namespace WebApp.Repositories;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User? GetUserById(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "SELECT Id, Username, PasswordHash, FirstName, LastName FROM Users WHERE Id = @Id";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4)
                    };
                }
            }
        }
        return null;
    }

    public User? GetUserByUserame(string username)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "SELECT Id, Username, PasswordHash, FirstName, LastName FROM Users WHERE Username = @Username";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4)
                    };
                }
            }
        }
        return null; 
    }

    public bool AddUser(User user)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "INSERT INTO Users (Username, FirstName, LastName, PasswordHash) VALUES (@Username, @FirstName, @LastName, @PasswordHash)";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName); 
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public User? GetUserByUsername(string username)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "SELECT Id, Username, PasswordHash, FirstName, LastName FROM Users WHERE Username = @Username";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserName", username);

            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4)
                    };
                }
            }
        }
        return null; 
    }

    public List<User> GetAllUsers()
    {
        var users = new List<User>();

        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "SELECT Id, Username, PasswordHash, FirstName, LastName FROM Users";
            var cmd = new SqlCommand(query, conn);

            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        FirstName = reader.GetString(3),
                        LastName = reader.GetString(4)
                    });
                }
            }
        }
        return users;
    }

    public bool UpdateUser(User user)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            var query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Id", user.Id);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0; 
        }
    }
}
