
namespace Shared.DTOs.Comments;

public class UpdateCommentDto
{
    public required int Id { get; set; }
    public required string? Body { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
}