using Microsoft.AspNetCore.Mvc;

namespace DotNetExamProject.Controllers;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly DatabaseContext _context;

    public RegisterController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUserModel(User userModel)
    {
        _context.Users.Add(userModel);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
