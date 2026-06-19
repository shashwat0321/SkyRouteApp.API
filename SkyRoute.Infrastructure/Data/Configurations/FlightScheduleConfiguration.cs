using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data.Configurations
{
    public class FlightScheduleConfiguration : IEntityTypeConfiguration<FlightSchedule>
    {
        public void Configure(EntityTypeBuilder<FlightSchedule> builder)
        {
            builder.Property(s => s.FlightNumber).IsRequired().HasMaxLength(20);
            builder.Property(s => s.DepartureTime).IsRequired();
            builder.Property(s => s.ArrivalTime).IsRequired();
            builder.Property(s => s.OperatingDays).IsRequired();
            builder.Property(s => s.EffectiveFrom).IsRequired();
            builder.Property(s => s.EffectiveTo).IsRequired();

            builder.Property(s => s.EconomySeats).IsRequired();
            builder.Property(s => s.BusinessSeats).IsRequired();
            builder.Property(s => s.FirstClassSeats).IsRequired();

            builder.Property(s => s.EconomyBasePrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(s => s.BusinessBasePrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(s => s.FirstClassBasePrice).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(s => s.Status).IsRequired();

            builder.HasOne(s => s.Airline)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.AirlineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.DepartureAirport)
                .WithMany()
                .HasForeignKey(s => s.DepartureAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.ArrivalAirport)
                .WithMany()
                .HasForeignKey(s => s.ArrivalAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Flights)
                .WithOne(f => f.FlightSchedule)
                .HasForeignKey(f => f.FlightScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
