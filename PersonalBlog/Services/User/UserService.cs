using PersonalBlog.DTOs.User;
using PersonalBlog.Models;
using PersonalBlog.Repositories;
using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher) : IUserService
{

    public async Task<ResponseUserDto> FindUserAsync(long id)
    {
        if (id <= 0) throw new ArgumentException("The Id is invalid.");
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
            Name = userDto.Name.Trim(),
            Email = userDto.Email.Trim(),
        };

        user.Password = passwordHasher.HashPassword(user, userDto.Password);

        await userRepository.CreateUserAsync(user);

        return ToResponseDto(user);
    }

    public async Task<ResponseUserDto> UpdateUser(UpdateUserDto userDto)
    {
        if (userDto.Id <= 0)
            throw new ArgumentException("The user id is invalid.");
        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("The user name cannot be empty.");
        if (string.IsNullOrWhiteSpace(userDto.Email))
            throw new ArgumentException("The user email cannot be empty.");

        var user = new User
        {
            Id = userDto.Id,
            Name = userDto.Name.Trim(),
            Email = userDto.Email.Trim()
        };

        var updated = await userRepository.UpdateUserAsync(user);

        if (!updated)
            throw new KeyNotFoundException("User not found.");

        return ToResponseDto(user);
    }

    public async Task DeleteUser(long id)
    {
        if (id <= 0) throw new ArgumentException("The Id is invalid.");
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
