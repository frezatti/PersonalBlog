using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PersonalBlog.DTOs.Auth;
using PersonalBlog.Models;
using PersonalBlog.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalBlog.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration
) : IAuthService
{
    public async Task<ResponseLoginDto> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Email))
            throw new ArgumentException("The email cannot be empty.");

        if (string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("The password cannot be empty.");

        var user = await userRepository.FindUserByEmailAsync(loginDto.Email.Trim());

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            loginDto.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = GenerateToken(user);

        return new ResponseLoginDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Type = user.Type,
            Token = token
        };
    }

    private string GenerateToken(User user)
    {
        var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is not configured.");

        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Type.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
