using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.Views;
using CRM.WPF;

namespace CRM.ViewModels
{
    class AddNewStockItemViewModel : ViewModelBase
    {
        private int id;
        private string name;
        private int manufacturerId;
        private string description;
        private Currency currency;
        private string notes;
        private float purchasePrice;
        private float retailPrice;
        private float quantity;

        public RelayCommand AddNewItemCommand { get; }
        public RelayCommand AddManufacturerCommand { get; }

        public int Id { get { return id; } set { id = value; OnPropertyChanged(); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public int ManufacturerId { get { return manufacturerId; } set { manufacturerId = value; OnPropertyChanged(); } }
        public string Description { get { return description; } set { description = value; OnPropertyChanged(); } }
        public Currency Currency { get { return currency; } set { currency = value; OnPropertyChanged(); } }
        public string Notes { get { return notes; } set { notes = value; OnPropertyChanged(); } }
        public float PurchasePrice { get { return purchasePrice; } set { purchasePrice = value; OnPropertyChanged(); } }
        public float RetailPrice { get { return retailPrice; } set { retailPrice = value; OnPropertyChanged(); } }
        public float Quantity { get { return quantity; } set { quantity = value; OnPropertyChanged(); } }
        public Database Database { get; set; }
        public StockRepository StockRepo { get; set; }
        public ManufacturerRepository ManufacturerRepo { get; set; }

        public AddNewStockItemViewModel() { }
        public AddNewStockItemViewModel(Database db, StockRepository sr, ManufacturerRepository mfr)
        {
            Database = db;
            StockRepo = sr;
            ManufacturerRepo = mfr;
            AddNewItemCommand = new RelayCommand(OnAddNewItemButtonClick);
            AddManufacturerCommand = new RelayCommand(OnAddManufacturerButtonClick);
        }

        void OnAddNewItemButtonClick(object _)
        {
            //var item = new StockItem(Id, Title, Currency, PurchasePrice, RetailPrice, Quantity);
            //mainViewModel.Database.StockItems.Add(item);

            //Title = null;
            //Currency = Currency;
            //PurchasePrice = 0;
            //Quantity = 0;
            //Id = Id + 1;
        }

        private void OnAddManufacturerButtonClick(object _)
        {
            var vm = new ManufacturerViewModel(Database, ManufacturerRepo);
            ManufacturerView ManufacturerView = new ManufacturerView();

            ManufacturerView.DataContext = vm;
            ManufacturerView.Show();
        }
    }
}
