using PersonalBlog.DTOs.User;
using PersonalBlog.Models;
using PersonalBlog.Repositories;

namespace PersonalBlog.Services;

public class UserService(IUserRepository userRepository): IUserService
{
    public async Task<ResponseUserDto> FindUserAsync(long id)
    {
        if(id<=0) throw new ArgumentException("The Id is invalid.");
        var user = await userRepository.FindUserAsync(id);
        
        return ToResponseDto(user);
    }

    public async Task<List<ResponseUserDto>> GetAllUserAsync()
    {
        
        var users = await userRepository.GetAllUserAsync();
        
        return users.Select(ToResponseDto).ToList();
    }
    
    public async Task<ResponseUserDto> CreateUser(CreateUserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("The User name cannot be empty.");
        if (string.IsNullOrWhiteSpace(userDto.Email))
            throw new ArgumentException("The User email cannot be empty.");
        if (string.IsNullOrWhiteSpace(userDto.Password))
            throw new ArgumentException("The User Password cannot be empty.");

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password,
        };
        
        await userRepository.CreateUserAsync(user);
        
        return ToResponseDto(user);
    }

    public async Task<ResponseUserDto> UpdateUser(UpdateUserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("The User name cannot be empty.");
        if (string.IsNullOrWhiteSpace(userDto.Email)) 
            throw new ArgumentException("The User email cannot be empty.");

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
        };
        await userRepository.UpdateUserAsync(user);

        return ToResponseDto(user);
    }

    public async Task DeleteUser(long id)
    {
        if(id<=0) throw new ArgumentException("The Id is invalid.");
        var deleted = await userRepository.DeleteUserAsync(id);
        
        if (!deleted) throw new KeyNotFoundException("User not Found");
    }
    
    private static ResponseUserDto ToResponseDto(User user)
    {
        return new ResponseUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Type = user.Type
        };
    }
}