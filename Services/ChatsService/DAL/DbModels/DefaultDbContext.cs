using ChatsService.DAL.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatsService.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<PrivateChat> PrivateChats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.ChatType).IsRequired();
            entity.HasMany(e => e.Messages)
                  .WithOne(m => m.Chat)
                  .HasForeignKey(m => m.ChatId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Content).IsRequired();
            entity.HasIndex(e => e.ChatId);
            entity.HasIndex(e => e.SenderId);
            entity.HasIndex(e => e.CreatedAt);
        });

        modelBuilder.Entity<PrivateChat>(entity =>
        {
            entity.HasIndex(e => new { e.Player1Id, e.Player2Id }).IsUnique();
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
