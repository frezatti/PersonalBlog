using PersonalBlog.DTOs.User;

namespace PersonalBlog.Services;

public interface IUserService
{
    Task<ResponseUserDto> CreateUser (CreateUserDto userDto);
    Task<ResponseUserDto> UpdateUser (UpdateUserDto userDto);
    Task DeleteUser (long Id);
    Task<ResponseUserDto> FindUserAsync(long id);
    Task<List<ResponseUserDto>> GetAllUserAsync();
}