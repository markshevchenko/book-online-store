using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BookDto> Books { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>()
                        .Property(dto => dto.DeliveryParameters)
                        .HasConversion(
                            value => JsonConvert.SerializeObject(value),
                            value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value));
                            
        }
    }
}
