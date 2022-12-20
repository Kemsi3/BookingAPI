using BookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-BC3PM33\\SQLEXPRESS;database=BookingSystem;trusted_connection=true;TrustServerCertificate=True");
        }

        public DbSet<Desk> Desks { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<User> Users { get; set; }
    }
}

