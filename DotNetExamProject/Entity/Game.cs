using System.ComponentModel.DataAnnotations;

namespace DotNetExamProject.Entity;

public class Game
{
    [Key]
    public int Id { get; set; }
    public string? OwnerUsername { get; set; }
    public DateTime CreatedOn { get; set; }
    // game state started | open
    public GameState State { get; set; }
}

public enum GameState
{
    Started,
    Open
}