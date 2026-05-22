using Microsoft.AspNetCore.Mvc;
using PersonalBlog.DTOs.Post;
using PersonalBlog.Services;
using Microsoft.AspNetCore.Authorization;

namespace PersonalBlog.Controllers;

[ApiController]
[Route("api/postagens")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<PostResponseDto>>> GetAllPosts()
    {
        var posts = await _postService.GetAllPostsAsync();

        return Ok(posts);
    }

    [AllowAnonymous]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostResponseDto>> FindPostById(long id)
    {
        try
        {
            var post = await _postService.FindPostAsync(id);

            return Ok(post);
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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostResponseDto>> CreatePost([FromBody] CreatePostDto postDto)
    {
        try
        {
            var post = await _postService.CreatePostAsync(postDto);

            return CreatedAtAction(
                nameof(FindPostById),
                new { id = post.Id },
                post
            );
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

    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<PostResponseDto>> UpdatePost(long id, [FromBody] UpdatePostDto postDto)
    {
        try
        {
            postDto.Id = id;

            var post = await _postService.UpdatePostAsync(postDto);

            return Ok(post);
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

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePost(long id)
    {
        try
        {
            await _postService.DeletePostAsync(id);

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
