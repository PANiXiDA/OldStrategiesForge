using GamesService.DAL.DbModels.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesService.DAL.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }

    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
