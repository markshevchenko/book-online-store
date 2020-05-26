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

        private string cellPhone;
        public string CellPhone
        {
            get { return cellPhone; }
            set
            {
                ThrowIfStateIsNot(OrderState.Created);

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CellPhone));

                cellPhone = value;
                State = OrderState.CellPhone;
            }
        }

        private OrderDelivery delivery;
        public OrderDelivery Delivery
        {
            get { return delivery; }
            set
            {
                ThrowIfStateIsNot(OrderState.CellPhone);

                if (value == null)
                    throw new ArgumentNullException(nameof(Delivery));

                State = OrderState.Delivery;
                delivery = value;
            }
        }

        private OrderPayment payment;
        public OrderPayment Payment
        {
            get { return payment; }
            set
            {
                ThrowIfStateIsNot(OrderState.Delivery);

                if (value == null)
                    throw new ArgumentNullException(nameof(Payment));

                State = OrderState.Payment;
                payment = value;
            }
        }

        public int TotalCount => Items.Sum(item => item.Count);

        public decimal TotalAmount => Items.Sum(item => item.Price * item.Count)
                                    + (Delivery?.Price ?? 0m);

        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            State = state;
            Items = new OrderItemCollection(items);
        }

        private void ThrowIfStateIsNot(OrderState state)
        {
            if (State != state)
                throw new InvalidOperationException("Invalid state.");
        }
    }
}
