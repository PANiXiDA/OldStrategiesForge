using EmailService.DAL.DAL.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.DAL.DAL.DbModels;

internal partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    internal virtual DbSet<NotificationSubscriberDbModel> NotificationSubscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationSubscriberDbModel>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
