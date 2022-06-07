using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    class StockItemViewModel : ViewModelBase
    {
        private int id;
        private string name;
        private Manufacturer manufacturer;
        private string description;
        private Currency currency;
        private float purchasePrice;
        private float retailPrice;
        private float quantity;
        private StockItem selectedStockItem;

        public RelayCommand SaveNewCommand { get; }
        public RelayCommand SaveEditCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; }

        public int Id { 
            get { return id; } 
            set { id = value; 
                OnPropertyChanged(); }
        }
        public string Name { 
            get { return name; } 
            set { name = value; 
                OnPropertyChanged(); }
        }
        public Manufacturer Manufacturer { 
            get { return manufacturer; } 
            set { manufacturer = value; 
                OnPropertyChanged(); } 
        }
        public string Description { 
            get { return description; } 
            set { description = value; 
                OnPropertyChanged(); } 
        }
        public Currency Currency {
            get { return currency; } 
            set { currency = value; 
                OnPropertyChanged(); } 
        }
        public float PurchasePrice { 
            get { return purchasePrice; } 
            set { purchasePrice = value; 
                OnPropertyChanged(); } 
        }
        public float RetailPrice { 
            get { return retailPrice; } 
            set { retailPrice = value; 
                OnPropertyChanged(); } 
        }
        public float Quantity { 
            get { return quantity; } 
            set { quantity = value; 
                OnPropertyChanged(); } 
        }
        public StockItem SelectedStockItem { 
            get { return selectedStockItem; } 
            set { selectedStockItem = value; 
                OnPropertyChanged(); } 
        }
        public string IsSaveNewButtonVisible { get; set; }
        public string IsSaveEditButtonVisible { get; set; }
        public Database Database { get; set; }
        public StockItemRepository StockRepo { get; set; }
        public ManufacturerRepository ManufacturerRepo { get; set; }
        public string WindowTitle { get; set; }

        public StockItemViewModel() { }
        public StockItemViewModel(Database db, StockItemRepository sr, ManufacturerRepository mfr, StockItem si, string saveNewVis, string saveEditVis)
        {
            Database = db;
            StockRepo = sr;
            ManufacturerRepo = mfr;
            SelectedStockItem = si;
            IsSaveNewButtonVisible = saveNewVis;
            IsSaveEditButtonVisible = saveEditVis;

            //SaveNewCommand = new RelayCommand(OnSaveNew);
            //SaveEditCommand = new RelayCommand(OnSaveEdit);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void CloseWindow(ICloseable window)
        {
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
