using Microsoft.AspNetCore.Mvc;
using Models;
using RepositoryContracts;
using Shared.DTOs.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("users")] 
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    static int nextId = 1;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    private async Task VerifyUserNameIsAvailableAsync(string username)
    {
        var existingUser = await Task.Run(() => userRepo.GetMany()
                                                      .FirstOrDefault(u => u.Username == username)); 

        if (existingUser != null) 
        {
            throw new InvalidOperationException($"Username '{username}' is already taken.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.Username);

        
        User user = new(nextId, request.Username, request.Password);

        nextId += 1;

        User created = await userRepo.AddAsync(user);

        UserDto dto = new()
        {
            Id = created.Id,
            Username = created.Username ?? "Guest"
        };
        
        return Created($"/users/{dto.Id}", dto); 
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser([FromRoute] int id)
    {
        User user = await userRepo.GetSingleAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        UserDto responseDto = new()
        {
            Id = user.Id,
            Username = user.Username ?? "Guest"
        };

        return Ok(responseDto);
    }
    
    [HttpGet]
    public ActionResult<List<UserDto>> GetManyUsers([FromQuery] string? username = null)
    {
        IQueryable<User> query = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(username))
        {
            query = query.Where(u => u.Username != null && u.Username.ToLower().Contains(username.ToLower()));
        }

        List<User> filteredUsers = query.ToList();

        List<UserDto> responseDtos = filteredUsers
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username ?? "Guest"
            })
            .ToList();

        return Ok(responseDtos);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("Route ID does not match User ID in body.");
        }

        User existingUser = await userRepo.GetSingleAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }
        

        existingUser.Username = request.Username;
        
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            existingUser.Password = request.Password; 
        }

       await userRepo.UpdateAsync(existingUser);

        UserDto responseDto = new()
        {
            Id = existingUser.Id,
            Username = existingUser.Username ?? "Guest"
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        if (await userRepo.GetSingleAsync(id) == null)
        {
            return NotFound(); 
        }

        await userRepo.DeleteAsync(id);

        return NoContent();
    }
}