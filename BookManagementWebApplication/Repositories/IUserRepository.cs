using BookManagementWebApplication.Model;

namespace BookManagementWebApplication.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
}