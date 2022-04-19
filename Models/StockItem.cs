using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockItem : Item
    {
        private int manufacturerId;
        private string description;
        private Currency currency;

        public int ManufacturerId { get { return manufacturerId; } set { manufacturerId = value; OnPropertyChanged(); } }
        public string Description { get { return description; } set { description = value; OnPropertyChanged(); } }
        public Currency Currency { get { return currency; } set { currency = value; OnPropertyChanged(); } }

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
