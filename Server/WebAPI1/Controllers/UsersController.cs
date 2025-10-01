

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

using RepositoryContracts;
// using Shared.DTOs.Posts;
// using Shared.DTOs.Comments;
using Shared.DTOs.Users;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    private async Task VerifyUserNameIsAvailableAsync(string username)
{
    var existingUser = await userRepo.GetMany()
                                     .FirstOrDefaultAsync(u => u.Username == username); 


    if (existingUser != null)                        
    {
        throw new InvalidOperationException($"Username '{username}' is already taken.");
    }
}


    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.Username);

        User user = new(01, request.Username, request.Password);

        User created = await userRepo.AddAsync(user);

        UserDto dto = new()
        {
            Id = created.Id,
            Username = created.Username ?? "Guest"
        };

        return Created($"/users/{dto.Id}", created);
        
    }

    
}