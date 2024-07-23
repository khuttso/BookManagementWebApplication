using BookManagementWebApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementWebApplication.Services;

public interface IUserAuthorizationService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<string> LoginAsync(LoginModel model);   
}