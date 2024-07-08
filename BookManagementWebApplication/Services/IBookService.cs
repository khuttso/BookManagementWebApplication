namespace BookManagementWebApplication.Services;
using Microsoft.AspNetCore.Mvc;
using BookManagementWebApplication.Model;
public interface IBookService
{
    Task<IEnumerable<Book>> GetBookAsync();
    Task<Book?> GetBookByIdAsync(int id); 
    Task<bool> AddBookAsync(Book book); 
    Task<bool> UpdateBookAsync(int id, Book book);
    Task<bool> DeleteBookAsync(int id);
}