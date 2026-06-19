using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.NumberOfSeats).IsRequired();
            builder.Property(b => b.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(b => b.Status).IsRequired();
            builder.Property(b => b.PaymentStatus).IsRequired();

            builder.HasMany(b => b.Passengers)
                .WithOne(p => p.Booking)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
