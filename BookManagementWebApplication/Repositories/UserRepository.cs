using BookManagementWebApplication.Data;
using BookManagementWebApplication.Model;
using BookManagementWebApplication.Services;

namespace BookManagementWebApplication.Repositories;
using Microsoft.EntityFrameworkCore;
public class UserRepository(UserDatabaseContext context) : IUserRepository
{
    private readonly UserDatabaseContext _context = context;
    
    
    
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(userItem => userItem.Username == username);
    }

    
    
    
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}