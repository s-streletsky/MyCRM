using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class StockItem : Item
    {
        private Manufacturer manufacturer;
        private string description;
        private Currency currency;

        public Manufacturer Manufacturer { get { return manufacturer; } set { manufacturer = value; OnPropertyChanged(); } }
        public string Description { get { return description; } set { description = value; OnPropertyChanged(); } }
        public Currency Currency { get { return currency; } set { currency = value; OnPropertyChanged(); } }

        public StockItem() { }
        public StockItem(string name, Manufacturer mf, string description, Currency curr, float purchasePrice, float retailPrice)
        {
            this.Name = name;
            this.Manufacturer = mf;
            this.Description = description;
            this.Currency = curr;
            this.PurchasePrice = purchasePrice;
            this.RetailPrice = retailPrice;
            this.Quantity = 0;
        }
        public StockItem(int id, string name, Manufacturer mf, string description, Currency currency, float purchasePrice, float retailPrice, float quantity)        
        {
            this.Id = id;
            this.Name = name;
            this.Manufacturer = mf;
            this.Description = description;
            this.Currency = currency;
            this.PurchasePrice = purchasePrice;
            this.RetailPrice = retailPrice;
            this.Quantity = quantity;
        }
    }
}
