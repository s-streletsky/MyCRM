using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{

    internal class Database
    {
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public ObservableCollection<OrderItem> OrdersItems { get; set; } = new ObservableCollection<OrderItem>();
        public ObservableCollection<StockItem> StockItems { get; set; } = new ObservableCollection<StockItem>();
        public ObservableCollection<StockArrival> StockArrivals { get; set; } = new ObservableCollection<StockArrival>();
        public ObservableCollection<ExchangeRate> ExchangeRates { get; set; } = new ObservableCollection<ExchangeRate>();
        public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new ObservableCollection<Manufacturer>();
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();
    }
}
