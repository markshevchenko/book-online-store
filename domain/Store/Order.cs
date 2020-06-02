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
                if (State != OrderState.Created && State != OrderState.Pushed)
                    throw new InvalidOperationException("Invalid state.");

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(CellPhone));

                cellPhone = value;
                State = OrderState.Pushed;
            }
        }

        private OrderDelivery delivery;
        public OrderDelivery Delivery
        {
            get { return delivery; }
            set
            {
                if (State != OrderState.Created && State != OrderState.Pushed)
                    throw new InvalidOperationException("Invalid state.");

                if (value == null)
                    throw new ArgumentNullException(nameof(Delivery));

                State = OrderState.Pushed;
                delivery = value;
            }
        }

        private OrderPayment payment;
        public OrderPayment Payment
        {
            get { return payment; }
            set
            {
                if (State != OrderState.Created && State != OrderState.Pushed)
                    throw new InvalidOperationException("Invalid state.");

                if (value == null)
                    throw new ArgumentNullException(nameof(Payment));

                State = OrderState.Pushed;
                payment = value;
            }
        }

        public int TotalCount => Items.Sum(item => item.Count);

        public decimal TotalAmount => Items.Sum(item => item.Price * item.Count)
                                    + (Delivery?.Price ?? 0m);

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Id = id;
            State = OrderState.Created;
            Items = new OrderItemCollection(this, items);
        }
    }
}
