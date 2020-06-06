using System.Collections.Generic;

namespace Store.Contractors
{
    public interface IDeliveryService
    {
        string Name { get; }

        string Title { get; }

        Form FirstForm(int orderId, IEnumerable<Book> books);

        Form NextForm(int step, IReadOnlyDictionary<string, string> values);

        OrderDelivery GetDelivery(Form form);
    }
}
