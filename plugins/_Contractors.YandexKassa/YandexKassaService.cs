using Store.Contractors;
using Web.Contractors;
using System;
using Store;
using System.Collections.Generic;
using System.Globalization;

namespace Contractors.YandexKassa
{
    public class YandexKassaService : IPaymentService, IWebService
    {
        public Guid Uid => Guid.Parse("{227A6101-8A55-497D-AD4D-0C36ADF2F9AD}");

        public string Title => "Оплата банковской картой через Яндекс.Кассу";

        public string PostUri => "/YandexKassa/Home/Start";

        public Form CreateForm(Order order)
        {
            var fields = new Field[]
            {
                new HiddenField("Номер заказа", "orderId", order.Id.ToString()),
                new HiddenField("Описание", "description", ""),
                new HiddenField("Сумма", "amount", order.TotalAmount.ToString(CultureInfo.InvariantCulture)),
            };

            return new Form(Uid, order.Id, 1, false, fields);
        }

        public OrderPayment GetPayment(Form form)
        {
            return new OrderPayment(Uid, "Оплата через Яндекс.Кассу", new Dictionary<string, string>());
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException();

            var fields = new Field[]
            {
                new HiddenField("Сумма", "amount", values["amount"]),
            };

            return new Form(Uid, orderId, 2, true, fields);
        }
    }
}
