using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    enum Status
    {
        NEW,
        Paid,
        Shipped
    }
    class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public Status Status { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }

        public Order()
        {
            Date = DateTime.Today;
            Items = new ObservableCollection<Item>();
        }
    }
}
