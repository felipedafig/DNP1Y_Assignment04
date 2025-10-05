
namespace Shared.DTOs.Users;

public class UpdateUserDto
{

    public required int Id { get; set; }
    public required string? Username { get; set; }
    public required string? Password { get; set; }
}