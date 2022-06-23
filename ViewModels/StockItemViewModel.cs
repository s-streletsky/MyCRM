using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private BindableFloat purchasePrice;
        private BindableFloat retailPrice;
        private float quantity;
        private StockItem selectedStockItem;

        public RelayCommand AddManufacturerCommand { get; }
        public RelayCommand<ICloseable> ComparePricesAndCloseCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; }

        public int Id { 
            get { return id; } 
            set { id = value; 
                OnPropertyChanged(); }
        }
        public string Name { 
            get { return name; } 
            set { name = value;
                //if (String.IsNullOrEmpty(value) || value.Length < 3)
                //{
                //    throw new ApplicationException("Минимальная длина - 3 символа");
                //}
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
        public BindableFloat PurchasePrice { 
            get { return purchasePrice; } 
            set { purchasePrice = value; 
                OnPropertyChanged(); } 
        }
        public BindableFloat RetailPrice { 
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
        public Database Database { get; set; }
        public StockItemRepository StockRepo { get; set; }
        public ManufacturerRepository ManufacturerRepo { get; set; }
        public string WindowTitle { get; set; }
        public bool IsOKButtonEnabled { get; set; }

        public StockItemViewModel() { }
        public StockItemViewModel(Database db, StockItemRepository sr, ManufacturerRepository mfr, StockItem si, string saveNewVis, string saveEditVis)
        {
            Database = db;
            StockRepo = sr;
            ManufacturerRepo = mfr;
            SelectedStockItem = si;

            PurchasePrice = new BindableFloat();
            RetailPrice = new BindableFloat();

            AddManufacturerCommand = new RelayCommand(OnAddManufacturer);
            ComparePricesAndCloseCommand = new RelayCommand<ICloseable>(ComparePricesAndClose);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void OnAddManufacturer()
        {
            var vm = new ManufacturerViewModel(Database, ManufacturerRepo);
            ManufacturerView manufacturerView = new ManufacturerView();

            manufacturerView.DataContext = vm;
            manufacturerView.Owner = App.Current.MainWindow;
            manufacturerView.ShowDialog();
        }

        private void ComparePricesAndClose(ICloseable window)
        {
            if (RetailPrice.Value >= PurchasePrice.Value)
                CloseWindow(window);
            else
                MessageBox.Show("Цена продажи не может быть ниже цены закупки!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
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
