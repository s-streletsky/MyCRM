using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;
using CRM.Models;

namespace CRM.ViewModels
{
    internal class StockArrivalViewModel : ViewModelBase
    {
        private StockItem stockItem;
        private float quantity;

        public RelayCommand AddStockArrivalCommand { get; }
        public RelayCommand AddOKCommand { get; }
        public RelayCommand AddCancelCommand { get; }
        public RelayCommand EditOKCommand { get; }
        public RelayCommand EditCancelCommand { get; }
        public Database Database { get; set; }
        public StockArrivalRepository StockArrivalRepo { get; set;}
        public StockItem StockItem { get { return stockItem; } set { stockItem = value; OnPropertyChanged(); } }
        public float Quantity { get { return quantity; } set { quantity = value; OnPropertyChanged(); } }
        public StockArrival SelectedArrival { get; set; }

        public StockArrivalViewModel(Database db, StockArrivalRepository sar)
        {
            Database = db;
            StockArrivalRepo = sar;

            AddStockArrivalCommand = new RelayCommand(OnAddStockArrivalClick);

            AddOKCommand = new RelayCommand(OnAddOKButtonClick);
        }

        private void OnAddStockArrivalClick(object _)
        {
            //var newArrival = new StockArrival()
        }

        private void OnAddOKButtonClick(object _)
        {
            var newArrival = StockArrivalRepo.Add(new StockArrival(StockItem, Quantity));
            Database.StockArrivals.Insert(0, newArrival);
        }
    }
}
