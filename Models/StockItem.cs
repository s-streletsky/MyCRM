using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockItem : Item
    {
        public int ManufacturerId { get; set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }

        public StockItem() { }
        public StockItem(int id, string name, int mfid, string desc, Currency currency, float purchasePrice, float retailPrice, float quantity)        
        {
            this.Id = id;
            this.Name = name;
            this.ManufacturerId = mfid;
            this.Description = desc;
            this.Currency = currency;
            this.PurchasePrice = purchasePrice;
            this.RetailPrice = retailPrice;
            this.Quantity = quantity;
        }
    }
}
