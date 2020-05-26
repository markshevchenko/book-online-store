using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string Code { get; }

        string Title { get; }

        Form CreateForm(Order order);

        Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values);

        OrderPayment GetPayment(Form form);
    }
}
