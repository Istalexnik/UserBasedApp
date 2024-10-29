using UserBasedApp.Models;

namespace UserBasedApp.Services;

public class AuthService
{
    private User? currentUser;

    public event Action? OnChange;

    public bool IsLoggedIn => currentUser != null;
    public User? CurrentUser => currentUser;

    // New property to check if the logged-in user is an admin
    public bool IsAdmin => currentUser?.Role == "Admin"; // Ensure Role is checked correctly

    public async Task<bool> LoginAsync(UserService userService, string username, string password)
    {
        var user = await userService.GetUserAsync(username);

        if (user != null && user.Password == password)
        {
            currentUser = user;
            NotifyStateChanged(); // Trigger event
            return true;
        }
        return false;
    }

    public void Logout()
    {
        currentUser = null;
        NotifyStateChanged(); // Trigger event on logout
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
