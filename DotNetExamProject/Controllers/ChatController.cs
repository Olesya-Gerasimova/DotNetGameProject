using DotNetExamProject.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private string GetUsername()
    {
        return User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
    }
    private readonly DatabaseContext _context;
    
    public ChatController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpPost("{gameId}")]
    public async Task<IActionResult> SendMessage(int gameId, [FromBody] string message)
    {
        var game = await _context.Games.FindAsync(gameId);
        if (game == null)
        {
            return NotFound();
        }
        if (!game.Players.Contains(GetUsername()))
        {
            return Unauthorized();
        }
        var newMessage = new Message
        {
            GameId = gameId,
            Sender = GetUsername(),
            Text = message,
            Time = DateTime.Now
        };
        _context.Messages.Add(newMessage);
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetMessages(int gameId)
    {
        var game = await _context.Games.FindAsync(gameId);
        if (game == null)
        {
            return NotFound();
        }
        if (!game.Players.Contains(GetUsername()))
        {
            return Unauthorized();
        }
        var messages = await _context.Messages.Where(m => m.GameId == gameId).ToListAsync();
        return Ok(messages);
    }
}
