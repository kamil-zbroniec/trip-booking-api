namespace TripBooking.Infrastructure;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class TripBookingDbContext(DbContextOptions<TripBookingDbContext> options) : DbContext(options)
{
    public virtual DbSet<TripEntity> Trips { get; set; }
    
    public virtual DbSet<TripRegistrationEntity> TripRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripEntity>(entity =>
        {
            entity.HasKey(x => x.Name);
            
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Start)
                .IsRequired();

            entity.Property(t => t.NumberOfSeats)
                .IsRequired();
        });

        modelBuilder.Entity<TripRegistrationEntity>(entity =>
        {
            entity.HasKey(x => new{ x.TripName, x.UserEmail });
            
            entity.Property(r => r.TripName)
                .IsRequired();
            
            entity.Property(r => r.UserEmail)
                .IsRequired();
            
            entity.HasOne(r => r.Trip)
                .WithMany(t => t.Registrations)
                .HasForeignKey(x => x.TripName)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}