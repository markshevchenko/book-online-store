using Store;
using Store.Contractors;
using System;
using System.Collections.Generic;

namespace Contractors.Postamate
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            { "1", "Москва" },
            { "2", "Санкт-Петербург" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Казанский вокзал" },
                    { "2", "Охотный ряд" },
                    { "3", "Савёловский рынок" },
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Московский вокзал" },
                    { "5", "Гостиный двор" },
                    { "6", "Петропавловская крепость" },
                }
            }
        };

        public string UniqueCode => "Postamate";

        public string Title => "Доставка через постаматы в Москве и Санкт-Петербурге";

        public Step CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Step(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("Город", "city", "1", cities),
            });
        }

        public OrderDelivery GetDelivery(Step form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (form.UniqueCode != UniqueCode || form.Number != 3 || !form.IsFinal)
                throw new InvalidOperationException("Unrecognized form data.");

            var cityId = form.Fields[0].Value;
            var city = cities[cityId];
            var postamateId = form.Fields[1].Value;
            var postamate = postamates[cityId][postamateId];

            var description = $"Город: {city}\nПостамат: {postamate}\n";
            var parameters = new { CityId = cityId, PostamateId = postamateId };

            return new OrderDelivery(UniqueCode, description, parameters, 100m);
        }

        public Step MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return new Step(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new FixedField("Город", "city", "1"),
                        new SelectionField("Постамат", "postamate", "1", postamates["1"]),
                    });
                }
                else if (values["city"] == "2")
                {
                    return new Step(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new FixedField("Город", "city", "2"),
                        new SelectionField("Постамат", "postamate", "4", postamates["2"]),
                    });
                }
                else
                    throw new InvalidOperationException("Invalid postamate city.");
            }
            else if (step == 2)
            {
                return new Step(UniqueCode, orderId, 3, true, new Field[]
                {
                    new FixedField("Город", "city", values["city"]),
                    new FixedField("Постамат", "postamate", values["postamate"]),
                });
            }
            else
                throw new InvalidOperationException("Invalid postamate step.");
        }
    }
}
