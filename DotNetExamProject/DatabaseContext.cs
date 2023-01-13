using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DatabaseContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=");
    }
}
