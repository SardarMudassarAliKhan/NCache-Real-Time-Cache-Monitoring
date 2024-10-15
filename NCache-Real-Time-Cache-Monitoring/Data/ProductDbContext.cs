using Microsoft.EntityFrameworkCore;
using NCache_Real_Time_Cache_Monitoring.Model;

namespace NCache_Real_Time_Cache_Monitoring.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
