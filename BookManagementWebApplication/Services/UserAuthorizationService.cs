using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BookManagementWebApplication.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BookManagementWebApplication.Services;

public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _service;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserAuthorizationService(IConfiguration configuration, IUserService service, IPasswordHasher<User> hasher)
    {
        _configuration = configuration;
        _service = service;
        _passwordHasher = hasher;
    }
    
    public async Task<string> RegisterAsync(RegisterModel model)
    {
        if (await _service.GetUserByUsername(model.Username) != null)
        {
            return String.Empty;
        }

        var user = new User()
        {
            Username = model.Username,
            PasswordHash = _passwordHasher.HashPassword(null, model.Password)
        };

        await _service.AddUserAsync(user);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var token = new JwtSecurityToken(issuer, audience, null, expires: DateTime.Now.AddMinutes(90), 
            signingCredentials: credentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }

    public async Task<string> LoginAsync(LoginModel model)
    {
        var user = await _service.GetUserByUsername(model.Username);
        if (user == null || ! _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password)
                .Equals(PasswordVerificationResult.Success))
        {
            return String.Empty;
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var token = new JwtSecurityToken(issuer, audience, null, expires: DateTime.Now.AddMinutes(90), 
            signingCredentials: credentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}