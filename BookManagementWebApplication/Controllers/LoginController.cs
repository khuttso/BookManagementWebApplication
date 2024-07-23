using BookManagementWebApplication.Model;
using BookManagementWebApplication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementWebApplication.Controllers;

[ApiController]
[Route("/api")]
public class LoginController : ControllerBase
{
    private readonly IUserAuthorizationService _authorizationService;

    public LoginController(IUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }


    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await _authorizationService.RegisterAsync(model);
        return result != String.Empty ? Ok(result) : BadRequest("Username already exists");
    }
    
    
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginRequest)
    {
        var token = await _authorizationService.LoginAsync(loginRequest);
        return token != String.Empty ? Ok(token) : Unauthorized();
    }



    
    /// TRASH
    // private bool IsValidUser(LoginModel loginModel)
    // {
    //     return loginModel.Username == "Admin" && loginModel.Password == "JigariAdmini"
    // }
}