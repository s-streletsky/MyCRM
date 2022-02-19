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
        public ObservableCollection<StockItem> Stock { get; set; } = new ObservableCollection<StockItem>();
        public ObservableCollection<Currency> Currencies { get; set; } = new ObservableCollection<Currency>();
    }
}
