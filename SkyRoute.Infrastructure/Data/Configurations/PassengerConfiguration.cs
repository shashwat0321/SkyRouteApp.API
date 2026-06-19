using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DateOfBirth).IsRequired();
            builder.Property(p => p.Nationality).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PassportNumber).HasMaxLength(20);
            builder.Property(p => p.NationalId).HasMaxLength(20);
        }
    }
}
