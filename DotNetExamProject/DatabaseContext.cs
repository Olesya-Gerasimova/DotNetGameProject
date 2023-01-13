using DotNetExamProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNetExamProject;

public class DatabaseContext : DbContext
{
    private readonly IConfiguration _config;
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Message> Messages { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration config) : base(options)
    {
        _config = config;
        base.Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<User>()
            .HasKey(u => u.Username);*/
        modelBuilder.Entity<Game>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Message>()
            .Property(f => f.MessageId)
            .ValueGeneratedOnAdd();
    }
}
