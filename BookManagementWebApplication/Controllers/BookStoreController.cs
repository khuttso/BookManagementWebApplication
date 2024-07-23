using BookManagementWebApplication.Services;
using BookManagementWebApplication.Data;
using Microsoft.AspNetCore.Mvc;
using BookManagementWebApplication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace BookManagementWebApplication.Controllers;

[ApiController]
[Route("/api/BookStore")]
[Authorize]
public class BookStoreController : ControllerBase
{
    /// <summary>
    ///     Attributes: BookStoreDatabaseContext _context - for doing database operations (get data, update, delete, ...)
    ///                 ILogger<BookStoreController> _logger - for logging.
    /// </summary>
    private readonly BookStoreDatabaseContext _context;
    private readonly ILogger<BookStoreController> _logger;
    private readonly IBookService _bookService;
    public BookStoreController(BookStoreDatabaseContext context, ILogger<BookStoreController> logger, IBookService bookService)
    {
        _context = context;
        _logger = logger;
        _logger.LogInformation($"Bookstore controller starts working");
        _bookService = bookService;
    }





    /// <summary>
    ///     GET: api/books
    ///       GetBooks() - method returns all the book data that is stored into the bookstore database
    ///       A logger object prints appropriate log information into the console
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetBooks")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        _logger.LogInformation("Getting all the data from bookstore");
        return await _context.Books.ToListAsync();
    }





    /// <summary>
    ///     GET: api/books/{id}
    ///       GetBook(int id) method searches the book in the bookstore that has book.Id = id
    ///       If there is no book with given id, logger calls LogWarning method with appropriate message
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetBook/{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var getBook = await _bookService.GetBookByIdAsync(id);
        if (getBook == null)
        {
            _logger.LogWarning($"The book with given Id = {id} has not found in the bookstore");
            return NotFound();
        }
        
        _logger.LogInformation($"The book with Id = {id} has been found and returned");
        return Ok(getBook);
    }





    /// <summary>
    ///     POST: api/books
    ///       
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    [HttpPost("PostBook")]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        var result = await _bookService.AddBookAsync(book);
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok(book);
    }





    /// <summary>
    ///     PUT: api/books/{id}
    ///     TODO - Description
    /// </summary>
    /// <param name="id"></param>
    /// <param name="book"></param>
    /// <returns></returns>
    [HttpPut("PutBook/{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        var result = await _bookService.UpdateBookAsync(id, book);
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok();
    }





    /// <summary>
    ///     DELETE: api/books/{id}
    ///     TODO - Description
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // 
    [HttpDelete("DeleteBook/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteBookAsync(id);
        if (!result)
        {
            return NotFound();
        }
        
        return Ok();
    }




    
    /// <summary>
    ///     BookExists(id) - auxiliary method figures out if the book with Id = id exists or not
    ///                      and return true or false respectively.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }





    [HttpGet("/Exception")]
    public IActionResult ThrowException()
    {
        throw new Exception("for testing");
    }
}