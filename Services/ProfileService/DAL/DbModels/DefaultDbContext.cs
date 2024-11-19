using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.DbModels.Models;

namespace ProfileService.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            entity.HasIndex(e => e.Nickname)
                .HasDatabaseName("Unique_Users_Login")
                .IsUnique();

            entity.Property(entity => entity.Nickname)
                .IsRequired();
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
