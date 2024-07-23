using BookManagementWebApplication.Model;

namespace BookManagementWebApplication.Services;

public interface IUserService
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByUsername(string username);
}   