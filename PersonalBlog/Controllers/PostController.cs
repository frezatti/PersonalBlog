using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.DTOs.Post;
using PersonalBlog.Services;

namespace PersonalBlog.Controllers;

[ApiController]
[Route("api/postagens")]
public class PostsController(IPostService postService) : ControllerBase
{
    private readonly IPostService postService;


    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<PostResponseDto>>> GetPosts(
        [FromQuery] long? userId,
        [FromQuery] long? topicId)
    {
        try
        {
            var posts = await postService.GetPostsAsync(userId, topicId);

            return Ok(posts);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [AllowAnonymous]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostResponseDto>> FindPostById(long id)
    {
        try
        {
            var post = await postService.FindPostAsync(id);

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
            var post = await postService.CreatePostAsync(postDto);

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

            var post = await postService.UpdatePostAsync(postDto);

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
            await postService.DeletePostAsync(id);

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
