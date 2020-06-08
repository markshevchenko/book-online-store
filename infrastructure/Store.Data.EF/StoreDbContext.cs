using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BookDto> Books { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public DbSet<OrderDto> Orders { get; set; }
    }
}
