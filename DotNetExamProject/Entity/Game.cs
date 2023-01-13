using System.ComponentModel.DataAnnotations;

namespace DotNetExamProject.Entity;

public class Game
{
    [Key]
    public int Id { get; set; }
    public string? OwnerUsername { get; set; }
    public DateTime CreatedOn { get; set; }
    public GameState State { get; set; }
    public List<string> Players { get; set; }
}

public enum GameState
{
    Started,
    Open
}