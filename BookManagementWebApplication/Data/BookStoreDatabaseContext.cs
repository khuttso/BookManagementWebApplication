using Microsoft.EntityFrameworkCore;
using BookManagementWebApplication.Model;
namespace BookManagementWebApplication.Data;


public class BookStoreDatabaseContext : DbContext
{
    public BookStoreDatabaseContext(DbContextOptions<BookStoreDatabaseContext> options) : base(options)
    {
    }
    
    
    /// <summary>
    ///     Todo - Description
    /// </summary>
    public DbSet<Book> Books { get; set; }
}