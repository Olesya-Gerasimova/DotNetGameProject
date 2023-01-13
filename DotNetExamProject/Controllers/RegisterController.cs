using Microsoft.AspNetCore.Mvc;

namespace DotNetExamProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly DatabaseContext _context;

    public RegisterController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public IActionResult Register([FromBody] User credentials)
    {
        if (credentials.Username is null || credentials.Password is null)
        {
            return BadRequest("Username or password is missing");
        }
        var user = _context.Users.FirstOrDefault(u => u.Username == credentials.Username);
        if (user is not null)
        {
            return BadRequest("User already exists");
        }
        _context.Users.Add(new User()
        {
            Username = credentials.Username,
            Password = PasswordHash.HashPassword(credentials.Password)
        });
        _context.SaveChanges();
        var result = new { success = true };
        return Ok(result);
    }
}
