using UserBasedApp.Data;
using UserBasedApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserBasedApp.Services;

public class ProjectService
{
    private readonly ProjectRepository _projectRepository;

    public ProjectService(ProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<List<Project>> GetProjectsByUserIdAsync(int userId) => _projectRepository.GetProjectsByUserIdAsync(userId);

    public Task<List<Project>> GetAllProjectsAsync() => _projectRepository.GetAllProjectsAsync();


    public Task AddProjectAsync(int userId, string name, string description) =>
        _projectRepository.AddProjectAsync(new Project { UserId = userId, Name = name, Description = description });
}
