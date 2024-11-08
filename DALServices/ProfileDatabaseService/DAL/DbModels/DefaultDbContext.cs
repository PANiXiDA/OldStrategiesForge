using Microsoft.EntityFrameworkCore;
using ProfileDatabaseService.DAL.DbModels.Moddels;

namespace ProfileDatabaseService.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public virtual DbSet<Player> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasIndex(e => e.Nickname)
                .HasDatabaseName("Unique_Users_Login")
                .IsUnique();

            entity.Property(entity => entity.Nickname)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Password).IsRequired();

            entity.Property(e => e.CreatedAt).HasColumnType("timestamp");

            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp");

            entity.Property(e => e.DeletedAt).HasColumnType("timestamp");

            entity.Property(e => e.LastLogin).HasColumnType("timestamp");
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
