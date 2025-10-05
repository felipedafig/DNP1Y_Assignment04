
namespace Shared.DTOs.Posts;

public class PostDto
{
    public required int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public required int UserId { get; set; }
}