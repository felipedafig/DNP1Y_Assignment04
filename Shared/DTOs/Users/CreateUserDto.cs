
namespace Shared.DTOs.Users;
public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; } //not good to send password without hashing back to client
}