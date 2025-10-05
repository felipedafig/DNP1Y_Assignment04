using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Shared.DTOs.Comments;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("comments")] 
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID does not match Comment ID in body.");
        }

        Comment existingComment = await commentRepo.GetSingleAsync(id);

        if (existingComment == null)
        {
            return NotFound();
        }

        existingComment.Body = request.Body;
        
        Comment updated = await commentRepo.UpdateAsync(existingComment);

        UpdateCommentDto responseDto = new()
        {
            Id = updated.Id,
            PostId = updated.PostId,
            Body = updated.Body,
            UserId = updated.UserId
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        if (await commentRepo.GetSingleAsync(id) == null)
        {
            return NotFound();
        }
        
        await commentRepo.DeleteAsync(id);

        return NoContent(); 
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment([FromRoute] int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        CommentDto responseDto = new()
        {
            Id = comment.Id,
            PostId = comment.PostId,
            Body = comment.Body,
            UserId = comment.UserId
        };

        return Ok(responseDto);
    }
    
    [HttpGet]
    public ActionResult<List<CommentDto>> GetManyComments([FromQuery] int? userId = null)
    {
        IQueryable<Comment> query = commentRepo.GetMany(); 

        if (userId.HasValue)
        {
            query = query.Where(c => c.UserId == userId.Value);
        }

        List<Comment> filtered = query.ToList(); 
        
        List<CommentDto> responseDtos = filtered
            .Select(c => new CommentDto
            {
                Id = c.Id,
                PostId = c.PostId,
                Body = c.Body,
                UserId = c.UserId
            })
            .ToList();


        return Ok(responseDtos);
    }
}