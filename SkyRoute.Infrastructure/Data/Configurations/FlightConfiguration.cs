using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.Property(f => f.FlightNumber).IsRequired().HasMaxLength(20);
            builder.Property(f => f.TotalSeats).IsRequired();
            builder.Property(f => f.AvailableSeats).IsRequired();
            builder.Property(f => f.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.Status).IsRequired();

            builder.HasOne(f => f.DepartureAirport)
                .WithMany()
                .HasForeignKey(f => f.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.ArrivalAirport)
                .WithMany()
                .HasForeignKey(f => f.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.Bookings)
                .WithOne(b => b.Flight)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
