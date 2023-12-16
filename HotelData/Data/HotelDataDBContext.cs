using HotelData.Data;
using HotelData.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace HotelData.Data
{
    public class HotelDataDBContext : DbContext
    {
        public HotelDataDBContext(DbContextOptions<HotelDataDBContext> options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HotelConfig());
        }
    }
}
