using GoBet.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

       
        builder.Entity<Booking>()
            .HasOne(b => b.Trip)
            .WithMany()
            .HasForeignKey(b => b.TripId);
    }
}