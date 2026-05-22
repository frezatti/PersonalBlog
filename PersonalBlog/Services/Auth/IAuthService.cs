using PersonalBlog.DTOs.Auth;

namespace PersonalBlog.Services;

public interface IAuthService
{
    Task<ResponseLoginDto> LoginAsync(LoginDto loginDto);
}
