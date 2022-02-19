using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockItem : Item
    {
        public string Manufacturer { get; set; }
        public string Notes { get; set; }
        public string Currency { get; set; }

        public StockItem()
        {
        }

        public StockItem(int id, string title, string currency, decimal purchasePrice, decimal retailPrice, double quantity)        {
            this.Id = id;
            this.Title = title;
            this.Currency = currency;
            this.PurchasePrice = purchasePrice;
            this.RetailPrice = retailPrice;
            this.Quantity = quantity;
        }
    }
}
