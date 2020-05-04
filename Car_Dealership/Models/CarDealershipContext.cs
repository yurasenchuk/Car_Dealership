using Microsoft.EntityFrameworkCore;

namespace Car_Dealership.Models
{
    public class CarDealershipContext:DbContext
    {
        public DbSet<Auto> Auto { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }

        public CarDealershipContext(DbContextOptions<CarDealershipContext> options) : base(options)
        {
            
        }
    }
}