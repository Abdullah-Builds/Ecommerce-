using Microsoft.EntityFrameworkCore;

namespace MyApp.Data
{

    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options) : base(options) 
            { }

    public DbSet<Order> OrdersConfirmed { get; set; }
    }
}
