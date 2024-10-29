using Microsoft.Data.Sqlite;
using UserBasedApp.Models;

namespace UserBasedApp.Data;

public static class DatabaseConfig
{
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, "UserBasedApp.db");

    public static void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={DatabasePath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT,
                UserId INTEGER NOT NULL,
                FOREIGN KEY(UserId) REFERENCES Users(Id) ON DELETE CASCADE
            );";
        command.ExecuteNonQuery();

     //   SeedAdminUser(connection); // Call to seed the initial admin
    }

    private static void SeedAdminUser(SqliteConnection connection)
    {
        // Check if any admin user exists
        var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
        var adminCount = (long)(checkCommand.ExecuteScalar() ?? 0L); // Safe unboxing

        if (adminCount == 0)
        {
            // If no admin exists, create a default admin user
            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
            INSERT INTO Users (Username, Password, Role)
            VALUES ($username, $password, $role)";
            insertCommand.Parameters.AddWithValue("$username", "admin");
            insertCommand.Parameters.AddWithValue("$password", "admin123"); // Store securely in production
            insertCommand.Parameters.AddWithValue("$role", "Admin");
            insertCommand.ExecuteNonQuery();
        }
    }
}
