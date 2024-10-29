using Microsoft.Data.Sqlite;
using UserBasedApp.Models;
using System.Threading.Tasks;

namespace UserBasedApp.Data;

public class UserRepository
{
    public UserRepository()
    {
        DatabaseConfig.InitializeDatabase();
    }

    public async Task<User?> GetUserAsync(string username)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Password, Role FROM Users WHERE Username = $username";
        command.Parameters.AddWithValue("$username", username);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }
        return null;
    }

    public async Task AddUserAsync(User user)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Username, Password, Role) 
            VALUES ($username, $password, $role)";
        command.Parameters.AddWithValue("$username", user.Username);
        command.Parameters.AddWithValue("$password", user.Password);
        command.Parameters.AddWithValue("$role", user.Role);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = new List<User>();
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Password, Role FROM Users";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            });
        }
        return users;
    }


    public async Task UpdateUserRoleAsync(int userId, string newRole)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Users
            SET Role = $role
            WHERE Id = $userId";
        command.Parameters.AddWithValue("$role", newRole);
        command.Parameters.AddWithValue("$userId", userId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<bool> HasAdminUserAsync()
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";

        var count = (long)(await command.ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

}
