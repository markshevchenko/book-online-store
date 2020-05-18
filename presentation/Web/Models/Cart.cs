namespace Web.Models
{
    public class Cart
    {
        public int OrderId { get; }

        public int TotalCount { get; set; }

        public decimal TotalAmount { get; set; }

        public Cart(int orderId)
        {
            OrderId = orderId;
            TotalCount = 0;
            TotalAmount = 0m;
        }
    }
}
