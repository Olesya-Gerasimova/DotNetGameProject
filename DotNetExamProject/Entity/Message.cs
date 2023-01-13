using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject.Entity;

[PrimaryKey(nameof(MessageId))]
public class Message
{
    [Key]
    public int GameId { get; set; }
    [Key]
    public int MessageId { get; set; }
    public string? Sender { get; set; }
    public string Text { get; set; }
    public DateTime Time { get; set; }
}
