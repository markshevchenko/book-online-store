using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        public OrderItemCollection Items { get; }

        public OrderState State { get; private set; }

        public string CellPhone { get; set; }

        public int TotalCount
        {
            get { return Items.Sum(item => item.Count); }
        }

        public decimal TotalAmount
        {
            get { return Items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            State = state;
            Items = new OrderItemCollection(items);
        }

        public void StartProcess()
        {
            ValidateState(OrderState.Created);

            State = OrderState.ProcessStarted;
        }

        private void ValidateState(OrderState state)
        {
            if (State != state)
                throw new InvalidOperationException("Invalid state.");
        }
    }
}
