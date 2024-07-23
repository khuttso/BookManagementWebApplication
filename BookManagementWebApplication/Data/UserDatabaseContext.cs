using Microsoft.EntityFrameworkCore;
using BookManagementWebApplication.Model;
namespace BookManagementWebApplication.Data;

public class UserDatabaseContext : DbContext
{
    public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
}