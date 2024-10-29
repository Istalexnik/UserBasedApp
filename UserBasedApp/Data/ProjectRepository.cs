using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserBasedApp.Models;

namespace UserBasedApp.Data;

public class ProjectRepository
{
    public ProjectRepository()
    {
        DatabaseConfig.InitializeDatabase();
    }

    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = new List<Project>();
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Description, UserId FROM Projects WHERE UserId = $userId";
        command.Parameters.AddWithValue("$userId", userId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            projects.Add(new Project
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                UserId = reader.GetInt32(3)
            });
        }
        return projects;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        var projects = new List<Project>();
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Description, UserId FROM Projects";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            projects.Add(new Project
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                UserId = reader.GetInt32(3)
            });
        }
        return projects;
    }


    public async Task AddProjectAsync(Project project)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Projects (Name, Description, UserId) 
            VALUES ($name, $description, $userId)";
        command.Parameters.AddWithValue("$name", project.Name);
        command.Parameters.AddWithValue("$description", project.Description);
        command.Parameters.AddWithValue("$userId", project.UserId);

        await command.ExecuteNonQueryAsync();
    }
}
