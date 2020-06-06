using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string Name { get; }

        string Title { get; }

        Form FirstForm(int orderId);

        Form NextForm(int step, IReadOnlyDictionary<string, string> values);

        OrderPayment GetPayment(Form form);
    }
}
