using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Contractors
{
    public class Form
    {
        public string Code { get; }

        public int OrderId { get; }

        public int Step { get; }

        public bool IsFinal { get; }

        public IReadOnlyList<Field> Fields { get; }

        public Form(string code, int orderId, int step, bool isFinal, IEnumerable<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException(nameof(code));

            if (step < 1)
                throw new ArgumentOutOfRangeException(nameof(step));

            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            Code = code;
            OrderId = orderId;
            Step = step;
            IsFinal = isFinal;
            Fields = fields.ToArray();
        }
    }
}
