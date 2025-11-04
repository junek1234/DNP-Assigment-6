using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;
    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        
        User? foundUser = await FindUserAsync(request.UserName);
        if (foundUser == null)
        {
            return Unauthorized();
        }
        if (request.Password!=foundUser.Password)
        {
            return Unauthorized();
        }
        UserDto dto = new()
        {
            Id = foundUser.Id,
            UserName = foundUser.Username
        };
        return dto;
    }
    private async Task<User?> FindUserAsync(string userName)
    {
        IEnumerable<User> users = userRepo.GetMany();
        foreach (User user in users)
        {
            if (user.Username.Equals(userName))
            {
                return user;
            }
        }
        return null;
        
    }
}
