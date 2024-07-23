using BookManagementWebApplication.Data;
using BookManagementWebApplication.Model;
using BookManagementWebApplication.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementWebApplication.Services;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;
    
    
    public async Task AddUserAsync(User user)
    {
        await _repository.AddUserAsync(user);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _repository.GetUserByUsernameAsync(username);
    }
}