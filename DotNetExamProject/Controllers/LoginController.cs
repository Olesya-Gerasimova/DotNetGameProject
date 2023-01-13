using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetExamProject.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNetExamProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DatabaseContext _context;

    public LoginController(IConfiguration config, DatabaseContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpPost]
    public IActionResult Login([FromBody] User credentials)
    {
        if (
            credentials.Username is null ||
            credentials.Password is null ||
            credentials.Username is "" ||
            credentials.Password is ""
            )
        {
            return BadRequest("Username or password is missing");
        }

        var user = AuthenticateUser(credentials);

        if (user is null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var result = new { token = GenerateJWT(user) };
        return Ok(result);
    }

    private string GenerateJWT(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "DotNetExamProject",
            audience: "User",
            claims: new List<Claim>
            {
                new Claim("Username", user.Username!)
            },
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User? AuthenticateUser(User credentials)
    {
        var user = _context.Users.FirstOrDefault(
            u => u.Username == credentials.Username && u.Password == UserHelper.HashPassword(credentials.Password)
        );
        return user;
    }
}
