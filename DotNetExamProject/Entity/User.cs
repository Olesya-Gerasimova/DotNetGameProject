using System.ComponentModel.DataAnnotations;

namespace DotNetExamProject.Entity;

public class User
{
    [Key]
    public string? Username { get; set; }
    // password hash
    public string? Password { get; set; }
}
