namespace Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public OrderItemModel[] Items { get; set; }

        public int TotalCount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
