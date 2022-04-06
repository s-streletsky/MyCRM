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
        public ObservableCollection<StockItem> Stock { get; set; } = new ObservableCollection<StockItem>();
        public ObservableCollection<ExchangeRate> ExchangeRates { get; set; } = new ObservableCollection<ExchangeRate>();
    }
}
