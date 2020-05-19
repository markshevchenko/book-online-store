using Store;
using System.Collections.Generic;

namespace Web.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public OrderState State { get; set; }

        public string CellPhone { get; set; }

        public OrderItemModel[] Items { get; set; } = new OrderItemModel[0];

        public int TotalCount { get; set; }

        public decimal TotalAmount { get; set; }

        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
