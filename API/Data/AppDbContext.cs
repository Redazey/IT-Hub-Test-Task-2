using API.Models;
using API.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API.Data;

public class AppDbContext : DbContext
{
    public DbSet<RollItem> RollItems => Set<RollItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEntity>()
            .HasQueryFilter(x => x.DeletedOn == null);
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
