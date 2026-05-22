using Microsoft.AspNetCore.Mvc;
using PersonalBlog.DTOs.Topic;
using PersonalBlog.Services;
using Microsoft.AspNetCore.Authorization;

namespace PersonalBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ResponseTopicDto>>> GetAllTopics()
    {
        var topics = await _topicService.GetAllTopicsAsync();

        return Ok(topics);
    }
    [AllowAnonymous]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ResponseTopicDto>> FindTopicById(long id)
    {
        try
        {
            var topic = await _topicService.FindTopicAsync(id);

            return Ok(topic);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ResponseTopicDto>> CreateTopic([FromBody] CreateTopicDto topicDto)
    {
        try
        {
            var topic = await _topicService.CreateTopicAsync(topicDto);

            return CreatedAtAction(
                nameof(FindTopicById),
                new { id = topic.Id },
                topic
            );
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ResponseTopicDto>> UpdateTopic(long id, [FromBody] UpdateTopicDto topicDto)
    {
        try
        {
            topicDto.Id = id;

            var topic = await _topicService.UpdateTopicAsync(topicDto);

            return Ok(topic);
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
    public async Task<IActionResult> DeleteTopic(long id)
    {
        try
        {
            await _topicService.DeleteTopicAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
