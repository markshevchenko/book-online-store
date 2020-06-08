using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Store.Data.EF
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly StoreDbContext dbContext;

        public OrderRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Order Create()
        {
            var dto = Order.Factory.CreateDto();
            dbContext.Orders.Add(dto);
            dbContext.SaveChanges();

            return Order.Mapper.ToDomain(dto);
        }

        public Order GetById(int id)
        {
            var dto = dbContext.Orders
                               .Include(order => order.Items)
                               .Single(order => order.Id == id);

            return Order.Mapper.ToDomain(dto);
        }

        public void Update(Order order)
        {
            dbContext.SaveChanges();
        }
    }
}
