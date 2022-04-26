using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class OrderItem
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public StockItem StockItem { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public float Total { get; set; }

        public OrderItem()
        {

        }
    }
}
