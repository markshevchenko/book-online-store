using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace Web
{
    public static class SessionExtensions
    {
        const string key = "Cart";

        public static void Set(this ISession session, Cart value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(value.OrderId);
                writer.Write(value.TotalCount);
                writer.Write(value.TotalAmount);

                session.Set(key, stream.ToArray());
            }
        }

        public static bool TryGetCart(this ISession session, out Cart value)
        {
            if (session.TryGetValue(key, out byte[] buffer))
            {
                using (var stream = new MemoryStream(buffer))
                using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
                {
                    int orderId = reader.ReadInt32();
                    int totalCount = reader.ReadInt32();
                    decimal totalAmount = reader.ReadDecimal();

                    value = new Cart(orderId)
                    {
                        TotalCount = totalCount,
                        TotalAmount = totalAmount,
                    };
                }

                return true;
            }

            value = null;
            return false;
        }
    }
}
