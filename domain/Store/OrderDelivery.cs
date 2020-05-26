using System;

namespace Store
{
    public class OrderDelivery
    {
        public string Code { get; }

        public string Description { get; }

        public object Parameters { get; }

        public decimal Price { get; }

        public OrderDelivery(string code, string description, object parameters, decimal price)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));

            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Code = code;
            Description = description;
            Parameters = parameters;
            Price = price;
        }
    }
}
