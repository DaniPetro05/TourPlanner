using Microsoft.EntityFrameworkCore;
using TourPlanner.Models;

namespace TourPlanner.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}

    public DbSet<User> Users => Set<User>();

    public DbSet<Tour> Tours => Set<Tour>();

    public DbSet<TourLog> TourLogs => Set<TourLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<User>()
            .HasMany(u => u.Tours)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);*/

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.TourLogs)
            .WithOne(l => l.Tour)
            .HasForeignKey(l => l.TourId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tour>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tours)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}