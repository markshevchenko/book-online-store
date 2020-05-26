using System;

namespace Store
{
    public class OrderPayment
    {
        public string Code { get; }

        public string Description { get; }

        public object Parameters { get; }

        public OrderPayment(string code, string description, object parameters)
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
        }
    }
}
