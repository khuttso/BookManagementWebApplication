using BookManagementWebApplication.Controllers;
using BookManagementWebApplication.Data;
using BookManagementWebApplication.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
namespace BookManagementWebApplication.Services;

public class BookService:IBookService
{
    private readonly BookStoreDatabaseContext _context;
    private readonly ILogger<BookService> _logger;

    public BookService(BookStoreDatabaseContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    
    
    /// <summary>
    ///     GetBookAsync() - method returns all the book data that is stored into the bookstore database
    ///     A logger object prints appropriate log information into the console
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Book>> GetBookAsync()
    {
        _logger.LogInformation("Getting all the data from bookstore");
        return await _context.Books.ToListAsync();
    }

    
    
    
    
    
    /// <summary>
    ///      GET: api/books/{id}
    ///       GetBook(int id) method searches the book in the bookstore such that book.Id = id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        _logger.LogInformation($"Trying to get book with Id = {id} from the bookstore");
        var book = await _context.Books.FindAsync(id); 
        
        return book;
    }

    
    
    
    
    /// <summary>
    ///     Adds given book into the bookstore
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public async Task<bool> AddBookAsync(Book book)
    {
        if (BookExists(book.Id))
        {
            return false;
        }
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Book with " +$"id = {book.Id}, " +
                               $"author = {book.Author}, " +
                               $" title = {book.Title}, " +
                               $"published year = {book.PublishedYear}, " +
                               $"genre = {book.Genre} " +
                               $"has been added to the bookstore successfully"
        );
        return true;
    }

    
    
    
    
    
    /// <summary>
    ///     UpdateBookAsync() - takes 2 parameters as an arguments. (int id, Book book) and updates book data by its id.
    ///         book that currently has Id = id, will change its attributes with given Book book attributes.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="book"></param>
    /// <returns></returns>
    public async Task<bool> UpdateBookAsync(int id, Book book)
    {
        if (id != book.Id)
        {
            _logger.LogWarning($"BadRequest: Book Id = {id} does not match Id in the request body");
            return false;
        }

        _logger.LogInformation("Trying to update the book data");
        var bookItem = await _context.Books.FindAsync(id);
        if (bookItem == null)
        {
            _logger.LogWarning($"The book with Id = {id} has not found, Invalid Id");
            return false;
        }
        bookItem.Id = book.Id;
        bookItem.Author = book.Author;
        bookItem.Title = book.Title;
        bookItem.PublishedYear = book.PublishedYear;
        bookItem.Genre = book.Genre;
        
        _context.Entry(bookItem).State = EntityState.Modified; 
        
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("The book is updated successfully");
            _logger.LogInformation("");
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!BookExists(id))
            {
                _logger.LogWarning($"NotFound: Book with ID = {id} does not exist");
                return false;
            }
            else
            {
                _logger.LogError(e, $"Concurrency exception occurred while updating book with ID {id}");
                throw;
            }
        }

        return true;
    }
    
    
    
    
    
    /// <summary>
    ///     DeleteBookAsync() - Deletes book with Id=id from the bookstore.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteBookAsync(int id)
    {
        _logger.LogInformation($"Trying to delete book with Id = {id} from the bookstore");
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {   
            _logger.LogWarning($"Book with Id = {id} has not found");
            return false;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"The book with id = {id} has been removed successfully");
        return true;
    }
    
    
    
    
    
    /// <summary>
    ///     BookExists(int id) - simply checks if the book with Id=id exists or not.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}
