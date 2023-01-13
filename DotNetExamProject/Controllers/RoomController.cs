using DotNetExamProject.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomController : ControllerBase
{
    private string GetUsername()
    {
        return User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
    }
    private readonly DatabaseContext _context;
    
    public RoomController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpPost("join/{id}")]
    public async Task<IActionResult> JoinRoom(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        if (game.State != GameState.Open)
        {
            return BadRequest("Game is not open");
        }
        if (game.Players.Contains(GetUsername()))
        {
            return BadRequest("You are already in this game");
        }
        game.Players.Add(GetUsername());
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("leave/{id}")]
    public async Task<IActionResult> LeaveRoom(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        if (!game.Players.Contains(GetUsername()))
        {
            return BadRequest("You are not in this game");
        }
        if (game.OwnerUsername == GetUsername())
        {
            return BadRequest("You can't leave this game as you are the owner of this game");
        }
        game.Players.Remove(GetUsername());
        await _context.SaveChangesAsync();
        return Ok();
    }
}
