using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Contractors
{
    public class Step
    {
        public string UniqueCode { get; }

        public int OrderId { get; }

        public int Number { get; }

        public bool IsFinal { get; }

        public IReadOnlyList<Field> Fields { get; }

        public Step(string uniqueCode, int orderId, int step, bool isFinal, IEnumerable<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(uniqueCode))
                throw new ArgumentException(nameof(uniqueCode));

            if (step < 1)
                throw new ArgumentOutOfRangeException(nameof(step));

            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            UniqueCode = uniqueCode;
            OrderId = orderId;
            Number = step;
            IsFinal = isFinal;
            Fields = fields.ToArray();
        }
    }
}
