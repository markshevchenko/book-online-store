using System;
using System.Collections.Generic;

namespace Store
{
    public class OrderPayment
    {
        public Guid Uid { get; }

        public string Description { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public OrderPayment(Guid uid, string description, IReadOnlyDictionary<string, string> parameters)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            Uid = uid;
            Description = description;
            Parameters = parameters;
        }
    }
}
