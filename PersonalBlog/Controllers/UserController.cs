using Microsoft.AspNetCore.Mvc;
using PersonalBlog.DTOs.User;
using PersonalBlog.DTOs.Auth;
using PersonalBlog.Services;
using Microsoft.AspNetCore.Authorization;

namespace PersonalBlog.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsersController(IUserService userService, IAuthService authService) : ControllerBase
{

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ResponseLoginDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await authService.LoginAsync(loginDto);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { message = exception.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ResponseUserDto>>> GetAllUsers()
    {
        var users = await userService.GetAllUserAsync();

        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ResponseUserDto>> FindUserById(long id)
    {
        try
        {
            var user = await userService.FindUserAsync(id);

            return Ok(user);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [HttpPost("cadastrar")]
    public async Task<ActionResult<ResponseUserDto>> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            var user = await userService.CreateUser(userDto);

            return CreatedAtAction(
                nameof(FindUserById),
                new { id = user.Id },
                user
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ResponseUserDto>> UpdateUser(long id, [FromBody] UpdateUserDto userDto)
    {
        try
        {
            userDto.Id = id;

            var user = await userService.UpdateUser(userDto);

            return Ok(user);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        try
        {
            await userService.DeleteUser(id);

            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
