using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
    public IActionResult Login([FromBody] User login)
    {
        IActionResult response = Unauthorized();
        var user = AuthenticateUser(login);

        if (user is not null)
        {
            var tokenString = GenerateJSONWebToken(user);
            response = Ok(new { token = tokenString });
        }

        return response;
    }

    private string GenerateJSONWebToken(User userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User AuthenticateUser(User login)
    {
        var user = new User();
        if (login.Username == "user")
        {
            user =  new User { Username = "User", Password = "" };
        }

        return user;
    }
}
