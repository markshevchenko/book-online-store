using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IPaymentService
    {
        string UniqueCode { get; }

        string Title { get; }

        Step CreateForm(Order order);

        Step MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values);

        OrderPayment GetPayment(Step form);
    }
}
