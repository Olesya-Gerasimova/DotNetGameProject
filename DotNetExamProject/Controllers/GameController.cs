using DotNetExamProject.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GameController : ControllerBase
{
    private string GetUsername()
    {
        return User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
    }
    private readonly DatabaseContext _context;
    
    public GameController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames(int page = 0)
    {
        var games = await _context.Games
            //.Where(g => g.State == GameState.Open || g.State == GameState.Started)
            .OrderBy(g => g.State)
            .Skip(page * 10)
            .Take(10)
            .ToListAsync();
    
        return games;
    }
    
    // get game by id
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
    
        if (game == null)
        {
            return NotFound();
        }
    
        return game;
    }

    [HttpPost]
    public async Task<ActionResult<Game>> PostGame()
    {
        var game = new Game
        {
            OwnerUsername = GetUsername(),
            CreatedOn = DateTime.Now,
            State = GameState.Open
        };
    
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
    
        return CreatedAtAction("GetGame", new { id = game.Id }, game);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Game>> DeleteGame(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
    
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
    
        return game;
    }
    
    [HttpPatch("{id}/started")]
    public async Task<ActionResult<Game>> GameStarted(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
    
        game.State = GameState.Started;
        _context.Entry(game).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    
        return game;
    }
}