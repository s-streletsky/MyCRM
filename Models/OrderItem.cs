using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class OrderItem : Item
    {
        public double Discount { get; set; }
        public double Subtotal { get; set; }
        public double Profit { get; set; }

        public OrderItem(int id, string title, decimal purchasePrice, decimal retailPrice, double quantity)
        {
            this.Id = id;
            this.Title = title;
            this.PurchasePrice = purchasePrice;
            this.RetailPrice = retailPrice;
            this.Quantity = quantity;
        }
    }
}
