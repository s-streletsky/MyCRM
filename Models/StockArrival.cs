using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;

namespace CRM.Models
{
    internal class StockArrival : ViewModelBase
    {
        private int id;
        private DateTime date;
        private StockItem stockItem;
        private float quantity;

        public int Id { get { return id; } set { id = value; OnPropertyChanged(); } }
        public DateTime Date { get { return date; } set { date = value; OnPropertyChanged(); } }
        public StockItem StockItem { get { return stockItem; } set { stockItem = value; OnPropertyChanged(); } }
        public float Quantity { get { return quantity; } set { quantity = value; OnPropertyChanged(); } }

        public StockArrival() { }
        public StockArrival(StockItem si, float quantity)
        {
            Date = DateTime.Now;
            StockItem = si;
            Quantity = quantity;
        }

        public StockArrival(int id, DateTime date, StockItem si, float quantity)
        {
            Id = id;
            Date = date;
            StockItem = si;
            Quantity=quantity;
        }
    }
}
