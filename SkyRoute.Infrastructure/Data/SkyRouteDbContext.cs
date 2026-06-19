using Microsoft.EntityFrameworkCore;
using SkyRoute.Domain.Entities;

namespace SkyRoute.Infrastructure.Data
{
    public class SkyRouteDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkyRouteDbContext).Assembly);
        }

    }
}

