using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DatabaseContext()
    {
        base.Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Username);
    }
}
