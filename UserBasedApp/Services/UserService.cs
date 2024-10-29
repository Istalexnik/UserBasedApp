using UserBasedApp.Data;
using UserBasedApp.Models;
using System.Threading.Tasks;

namespace UserBasedApp.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> GetUserAsync(string username) => _userRepository.GetUserAsync(username);

    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }


    public async Task AddUserAsync(string username, string password, string role)
    {
        if (role != "User" && role != "Admin")
        {
            throw new ArgumentException("Invalid role specified");
        }

        await _userRepository.AddUserAsync(new User { Username = username, Password = password, Role = role });
    }



    public Task UpdateUserRoleAsync(int userId, string newRole)
    {
        return _userRepository.UpdateUserRoleAsync(userId, newRole);
    }

    public Task<bool> HasAdminUserAsync()
    {
        return _userRepository.HasAdminUserAsync();
    }

}
