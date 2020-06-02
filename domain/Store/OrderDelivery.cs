using System;
using System.Collections.Generic;

namespace Store
{
    public class OrderDelivery
    {
        public Guid Uid { get; }

        public string Description { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public decimal Price { get; }

        public OrderDelivery(Guid uid, string description, IReadOnlyDictionary<string, string> parameters, decimal price)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Uid = uid;
            Description = description;
            Parameters = parameters;
            Price = price;
        }
    }
}
