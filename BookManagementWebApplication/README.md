# BookStore

## Description
A brief overview of what the project does.

## Table of Contents
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Logging](#logging)
- [Libraries and Dependencies](#libraries-and-dependencies)
- [Testing](#Testing)

## Usage
With bookstore web application user can store some book information into the database.
Bookstore supports following operations: 
- Adding a book to the bookstore.
- Removing a book from the bookstore
- Updating existing book data
- Getting information about some book
- Getting all the book that currently is into the bookstore





## API Documentation
### Endpoints
- **GET /api/GetBooks** - Retrieves all books from the bookstore 
- **GET /api/GetBook/{id}** - Retrieve a specific book by ID.
- **POST /api/books** - Add a new book.
- **PUT /api/books/{id}** - Update an existing book.
- **DELETE /api/books/{id}** Delete a book.


### Methods
```csharp
 [HttpGet("/GetBooks")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        ///
    }
```
Method `GetBooks()` retrieves all the book data

```csharp
[HttpGet("/GetBook/{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        ///    
    }
```
Method `GetBook(int id)` Retrieves book by given id
```csharp
    [HttpPost("/PostBook")]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        ///
    }
```
Method `PostBook(Book book)` takes book object as an argument an adds it into the bookstore
```csharp
[HttpPut("/PutBook/{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        ///
    }
```
Method `PutBook(int id, Book book)` takes integer id and Book object as an attribute. Updates book attributes with given book. 
```csharp
[HttpDelete("/DeleteBook/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {  
        ///
    }
```
Method `DeleteBook(id)` simply deletes book with given id from the bookstore

## Logging
```csharp
private readonly ILogger<BookStoreController> _logger;
```
This logger logs all the information about requests and responses, including database CRUD operations.

**examples in the code:** 
```csharp
    _logger.LogInformation("Getting all the data from bookstore");
    
    _logger.LogInformation($"Trying to get book with Id = {id} from the bookstore");
    
    _logger.LogInformation($"Book with " +
        $"id = {book.Id}, " +
        $"author = {book.Author}, " +
        $" title = {book.Title}, " +
        $"published year = {book.PublishedYear}, " +
        $"genre = {book.Genre} " +
        $"has been added to the bookstore successfully"
    );
    
    ...
```

## Services
### BookService
Class for handling CRUD operations on books.
- Methods
  - `Task<IEnumerable<Book>> GetBookAsync()`;
  - `Task<Book?> GetBookByIdAsync(int id)`;
  - `Task<bool> AddBookAsync(Book book)`;
  - `Task<bool> UpdateBookAsync(int id, Book book)`;
  - `Task<bool> DeleteBookAsync(int id);`

Based on IService interface.
Each method has its description attached as a comment.
## Libraries and Dependencies
- `Microsoft.EntityFrameworkCore;`          `Version - 8.0.6` 
- `Microsoft.EntityFrameworkCore.Tools;`    `Version - 8.0.6` 
- `Microsoft.EntityFrameworkCore.InMemory;` `Version - 8.0.6` 
- `Microsoft.EntityFrameworkCore.Sqlite`    `Version - 8.0.6`
- `Microsoft.AspNetCore.OpenApi`            `Version - 8.0.6`
- `Moq`                                     `Version - 4.20.70`
- `Swashbuckle.AspNetCore`                  `Version - 6.4.0`

## Testing

### BookStoreControllerTests
Unit test class for testing web api methods: PUT, GET, POST, DELETE.

#### Testcases
```csharp
 [Fact]
 public async Task GetBookTest()
 {
    ///
 }
```
```text
Initially, the book object with attributes:
      Title = "GetTestTitle",
      Author = "GetTestAuthor1",
      PublishedYear = 3001,
      Genre = "GetTestGenre1"
    is added to the bookstore (In-memory database), then this testcase checks
    if GetBook() methods of the class BookStoreController works correctly and if the
    book that is found with some id is equal to the book that was added with this id
```

```csharp
 [Fact]
 public async Task GetBookWhenNotFoundTest()
 {
     var result = await _controller.GetBook(Int32.MaxValue);
     Assert.IsType<NotFoundResult>(result.Result);
 }
```
```text
 GetBookWhenNotFoundTest() method tests if NotFound() method is called as it is expected
 Just for testing here is called GetBook method with id int32.MaxValue.
 For passing this testcase we should not sure that there is no book with this id in the database. 
```
```csharp
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
```
```text
 PostBookTest() method tests if the book is posted as expected.
 Initially the book object with attributes:
    book: Title = "PostBookTitle1",
    Author = "PostBookAuthor1",
    PublishedYear = 3002,
    Genre = "PostBookGenre1",
    is added to the bookstore with PostBook method of the BookStoreController object
    Assertions : oldLength + 1 = newLength where oldLength and newLength are data sizes before and after posting book resp.
    book that is found by id = book.id is equal to the book.
```
```csharp
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
```
```text
  DeleteBookTest() method tests if the book is properly deleted from the bookstore
  Initially, the book object with attributes:
  book: Title = "TTitle",
  Author = "AAuthor",
  PublishedYear = 3010,
  Genre = "GGenre"
  is added to the bookstore. Then the book object is removed and checked if occurrence differences between
  before and after deleting is equal to 1. 
    
```

```csharp
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
```
```text
 UpdateBookTest() method tests if the PutBook() method of the BookStoreController class works correctly.
 we have 2 book instances.
 book:
        Author = "Author1",
        Title = "Author1",
        PublishedYear = 3003, 
        Genre = "Genre1"
 updatedBook:
        Author = "UpdatedAuthor1",
        Title = "UpdatedAuthor1",
        PublishedYear = 3004,
        Genre = "UpdatedGenre1"
    Initially, book object is added to the bookstore. Then here is performed update operation
    by using PostBook() method of the BookStoreController class and checked if the length changes
    or attributes of the updated object are updated as expected. (using Equals method of the class Book)
```