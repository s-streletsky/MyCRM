using CRM.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    abstract class Item : ViewModelBase
    {
        private int id;
        private string name;
        private float purchasePrice;
        private float retailPrice;
        private float quantity;

        public int Id { get { return id; } set { id = value; OnPropertyChanged(); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public float PurchasePrice { get { return purchasePrice; } set { purchasePrice = value; OnPropertyChanged(); } }
        public float RetailPrice { get { return retailPrice; } set { retailPrice = value; OnPropertyChanged(); } }
        public float Quantity { get { return quantity; } set { quantity = value; OnPropertyChanged(); } }
    }
}
