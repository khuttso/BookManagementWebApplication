
using BookManagementWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit.Abstractions;
using Xunit;
using Moq;
using BookManagementWebApplication.Model;
using BookManagementWebApplication.Data;
using BookManagementWebApplication.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace BookManagementTests;

/// <summary>
///     Class <c>BookStoreControllerTests</c> - For testing controller operations:
///         GET, PUT, DELETE, POST
///     Each testcase that is appeared in this class has proper names and descriptions above them
/// </summary>
public class BookStoreControllerTests
{

    private readonly BookStoreDatabaseContext _context;
    private readonly Mock<ILogger<BookStoreController>> _loggerMock;
    private readonly Mock<ILogger<BookService>> _bookServiceLoggerMock;
    private readonly BookStoreController _controller;

    
    
    /// <summary>
    ///     Initialization attributes for database context, logger and bookstore controller
    ///
    ///     For testing here is used InMemoryDatabase with name "TestDatabase"
    /// </summary>
    public BookStoreControllerTests()
    {
        var options = new DbContextOptionsBuilder<BookStoreDatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new BookStoreDatabaseContext(options);
        _loggerMock = new Mock<ILogger<BookStoreController>>();
        _bookServiceLoggerMock = new Mock<ILogger<BookService>>();
        _controller = new BookStoreController(_context, _loggerMock.Object, new BookService(_context, _bookServiceLoggerMock.Object));
    }
    
    
    
    
    
    /// <summary>
    ///        GetBookTest()
    ///        Initially, the book object with attributes:
    ///             Title = "GetTestTitle",
    ///             Author = "GetTestAuthor1",
    ///             PublishedYear = 3001,
    ///             Genre = "GetTestGenre1"
    ///         is added to the bookstore (In-memory database), then this testcase checks
    ///         if GetBook() methods of the class BookStoreController works correctly and if the
    ///         book that is found with some id is equal to the book that was added with this id 
    /// </summary>
    [Fact]
    public async Task GetBookTest()
    {
        int oldLength = (await _controller.GetBooks()).Value.Count();
        Book book = new Book()
        {
            Title = "GetTestTitle",
            Author = "GetTestAuthor1",
            PublishedYear = 3001,
            Genre = "GetTestGenre1"
        };

        await _controller.PostBook(book);
        int newLength = (await _controller.GetBooks()).Value.Count();
        var result = await _controller.GetBook(book.Id);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var val = Assert.IsType<Book>(ok.Value);
        
        Assert.Equal(oldLength + 1, newLength);
        Assert.Equal(book.Id, val.Id);
        Assert.True(book.Equals(val));
    }

    
    
    
    
    /// <summary>
    ///     GetBookWhenNotFoundTest() method tests if NotFound() method is called as it is expected
    ///     Just for testing here is called GetBook method with id int32.MaxValue.
    ///     For passing this testcase we should not sure that there is no book with this id in the database. 
    /// </summary>
    [Fact]
    public async Task GetBookWhenNotFoundTest()
    {
        var result = await _controller.GetBook(Int32.MaxValue);
        Assert.IsType<NotFoundResult>(result.Result);
    }
    

    
    
    
    /// <summary>
    ///     PostBookTest() method tests if the book is posted as expected.
    ///     Initially the book object with attributes:
    ///   book: Title = "PostBookTitle1",
    ///         Author = "PostBookAuthor1",
    ///         PublishedYear = 3002,
    ///         Genre = "PostBookGenre1",
    ///     is added to the bookstore with PostBook method of the BookStoreController object
    ///     Assertions : oldLength + 1 = newLength where oldLength and newLength are data sizes before and after posting book resp.
    ///                  book that is found by id = book.id is equal to the book.
    /// </summary>
    [Fact]
    public async Task PostBookTest()
    {
        int oldLength = (await _controller.GetBooks()).Value.Count();
        
        Book book = new Book()
        {
            Title = "PostBookTitle1",
            Author = "PostBookAuthor1",
            PublishedYear = 3002,
            Genre = "PostBookGenre1",
        };
        
        await _controller.PostBook(book);
        var getBook = await _context.Books.FindAsync(book.Id);
        int newLength = (await _controller.GetBooks()).Value.Count();
        Assert.Equal(oldLength + 1, newLength);
        Assert.True(getBook.Equals(book));
    }
    
    
    
    
    
    /// <summary>
    ///     DeleteBookTest() method tests if the book is properly deleted from the bookstore
    ///     Initially, the book object with attributes:
    ///   book: Title = "TTitle",
    ///         Author = "AAuthor",
    ///         PublishedYear = 3010,
    ///         Genre = "GGenre"
    ///     is added to the bookstore. Then the book object is removed and checked if occurrence differences between
    ///     before and after deleting is equal to 1. 
    ///       
    /// </summary>
    [Fact]
    public async Task DeleteBookTest()
    {
        Book book = new Book()
        {
            Title = "TTitle",
            Author = "AAuthor",
            PublishedYear = 3010,
            Genre = "GGenre"
        };
        await _controller.PostBook(book);
        int count = (await _controller.GetBooks()).Value.Count(bookItem => bookItem.Equals(book));
        var book1 = (await _controller.GetBook(book.Id)).Value;
        Assert.True(book != null);
        
        await _controller.DeleteBook(book.Id);
        int newCount = (await _controller.GetBooks()).Value.Count(bookItem => bookItem.Equals(book));
        
        Assert.Equal(count-1, newCount);
    }

    
    
    
    
    /// <summary>
    ///     UpdateBookTest() method tests if the PutBook() method of the BookStoreController class works correctly.
    ///         we have 2 book instances.
    ///        book:
    ///             Author = "Author1",
    ///              Title = "Author1",
    ///              PublishedYear = 3003, 
    ///              Genre = "Genre1"
    ///  updatedBook:
    ///              Author = "UpdatedAuthor1",
    ///              Title = "UpdatedAuthor1",
    ///              PublishedYear = 3004,
    ///              Genre = "UpdatedGenre1"
    ///         Initially, book object is added to the bookstore. Then here is performed update operation
    ///         by using PostBook() method of the BookStoreController class and checked if the length changes
    ///         or attributes of the updated object are updated as expected. (using Equals method of the class Book)
    /// </summary>
    [Fact]
    public async Task UpdateBookTest()
    {
        Book book = new Book()
        {
            Author = "Author1",
            Title = "Author1",
            PublishedYear = 3003, 
            Genre = "Genre1"
        };
        Book updatedBook = new Book()
        {
            Author = "UpdatedAuthor1",
            Title = "UpdatedAuthor1",
            PublishedYear = 3004,
            Genre = "UpdatedGenre1"
        };
        await _controller.PostBook(book);
        int oldLength = (await _controller.GetBooks()).Value.Count();

        updatedBook.Id = book.Id;
        await _controller.PutBook(book.Id, updatedBook);
        int newLength = (await _controller.GetBooks()).Value.Count();
        Assert.Equal(oldLength, newLength);
        
        var getBook = (await _context.Books.FindAsync(book.Id));
        Assert.True(getBook != null);
        Assert.True(getBook.Equals(updatedBook));
    }
}