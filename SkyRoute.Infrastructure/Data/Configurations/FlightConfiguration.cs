using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.Property(f => f.FlightDate).IsRequired();

            builder.Property(f => f.EconomyAvailable).IsRequired();
            builder.Property(f => f.BusinessAvailable).IsRequired();
            builder.Property(f => f.FirstClassAvailable).IsRequired();

            builder.Property(f => f.Status).IsRequired();

            // Concurrency token
            builder.Property(f => f.RowVersion).IsRowVersion();

            // One flight instance per schedule per date
            builder.HasIndex(f => new { f.FlightScheduleId, f.FlightDate }).IsUnique();

            builder.HasMany(f => f.Bookings)
                .WithOne(b => b.Flight)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
