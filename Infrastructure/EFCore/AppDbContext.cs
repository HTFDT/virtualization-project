using System.Reflection;
using Domain.Interfaces;
using Domain.Types;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ThreadEntity> Threads { get; set; }
    public DbSet<RatingEntry> Ratings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entities = Assembly.GetAssembly(typeof(Domain.AssemblyRef))!
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEntity)) && t.IsClass)
            .ToList();

        foreach (var entity in entities)
        {
            if (entity.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>)))
            {
                modelBuilder.Entity(entity).HasKey("PKey");
            }
            if (entity.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<,>)))
            {
                modelBuilder.Entity(entity).HasKey("PKey1", "PKey2");
            }
        }

        modelBuilder.Entity<RatingEntry>()
            .HasOne(r => r.Thread)
            .WithMany(t => t.Ratings)
            .HasForeignKey(r => r.ThreadId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}