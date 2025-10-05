using Microsoft.AspNetCore.Mvc;
using Models;

using RepositoryContracts;
using Shared.DTOs.Posts;
using Shared.DTOs.Comments;


[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;

    static int nextId = 1;
    static int commentNextId = 1;

    public PostsController(IPostRepository postRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        Post post = new Post(nextId, request.Title, request.Body, request.UserId);

        nextId += 1;

        Post created = await postRepo.AddAsync(post);

        PostDto responseDto = new()
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/posts/{responseDto.Id}", responseDto);

    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID does not match Post ID in body.");
        }

        Post existingPost = await postRepo.GetSingleAsync(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        existingPost.Title = request.Title;
        existingPost.Body = request.Body;
        existingPost.UserId = request.UserId;

        await postRepo.UpdateAsync(existingPost);

        UpdatePostDto responseDto = new()
        {
            Id = existingPost.Id,
            Title = existingPost.Title,
            Body = existingPost.Body,
            UserId = existingPost.UserId
        };

        return Ok(responseDto);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost([FromRoute] int id)
    {
        Post post = await postRepo.GetSingleAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        PostDto responseDto = new()
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };

        return Ok(responseDto);
    }

    [HttpGet]
    public ActionResult<List<PostDto>> GetManyPosts([FromQuery] string? contains = null, [FromQuery] int? userId = null)
    {
        List<PostDto> responseDtos = new List<PostDto>();

        IQueryable<Post> query = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(contains))
        {
            query = query.Where(p => p.Title != null && p.Title.Contains(contains));
        }

        if (userId.HasValue)
        {
            query = query.Where(p => p.UserId == userId.Value);
        }

        List<Post> filtered = query.ToList();

        foreach (Post p in filtered)
        {
            PostDto responseDto = new()
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.UserId
            };

            responseDtos.Add(responseDto);
        }

        return Ok(responseDtos);
    }

    [HttpPost("{postId:int}/comments")]
    public async Task<ActionResult<CommentDto>> AddCommentToPost([FromRoute] int postId,[FromBody] CreateCommentDto request)
    {
        if (await postRepo.GetSingleAsync(postId) == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        Comment comment = new Comment(commentNextId, request.Body ?? "Empty", request.UserId, postId);
        
        commentNextId += 1;

        Comment created = await commentRepo.AddAsync(comment);

        CommentDto responseDto = new()
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId
        };

        return Created($"/posts/{postId}/comments/{responseDto.Id}", responseDto);
    }

    [HttpGet("{postId:int}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsOfPost([FromRoute] int postId)
    {
        if (await postRepo.GetSingleAsync(postId) == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        List<Comment> comments =  commentRepo.GetMany().Where(p => p.PostId == postId).ToList();

        List<CommentDto> responseDtos = comments
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        Post existingPost = await postRepo.GetSingleAsync(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        await postRepo.DeleteAsync(id);

        return NoContent();
    }



    









}