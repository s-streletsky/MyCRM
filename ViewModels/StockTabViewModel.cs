using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRM.ViewModels
{
    internal class StockTabViewModel : ViewModelBase
    {
        private ObservableCollection<StockItem> dbStockItems;
        private ObservableCollection<StockArrival> dbStockArrivals;
        private ObservableCollection<Manufacturer> dbManufacturers;
        private ObservableCollection<ExchangeRate> dbExchangeRates;
        private StockItem selectedStockItem;

        private StockItemRepository stockItemRepo;
        private StockArrivalRepository stockArrivalRepo;
        private ManufacturerRepository manufacturerRepo;
        private ExchangeRateRepository exchangeRateRepo;

        public RelayCommand AddStockItemCommand { get; }
        public RelayCommand EditStockItemCommand { get; }
        public RelayCommand DeleteStockItemCommand { get; }
        public RelayCommand EditManufacturerCommand { get; }
        public RelayCommand EditStockArrivalCommand { get; }
        public RelayCommand EditExchangeRatesCommand { get; }

        public ObservableCollection<StockItem> DbStockItems { get { return dbStockItems; } }
        public StockItem SelectedStockItem
        {
            get { return selectedStockItem; }
            set
            {
                selectedStockItem = value;
                OnPropertyChanged(nameof(IsStockButtonsEnabled));
            }
        }
        public bool IsStockButtonsEnabled
        {
            get { return SelectedStockItem != null; }
        }

        public StockTabViewModel(ObservableCollection<StockItem> si, ObservableCollection<StockArrival> sa, ObservableCollection<Manufacturer> m, ObservableCollection<ExchangeRate> er, StockItemRepository sir, StockArrivalRepository sar, ManufacturerRepository mr, ExchangeRateRepository err)
        {
            dbStockItems = si;
            dbStockArrivals = sa;
            dbManufacturers = m;
            dbExchangeRates = er;

            stockItemRepo = sir;
            stockArrivalRepo = sar;
            manufacturerRepo = mr;
            exchangeRateRepo = err;

            AddStockItemCommand = new RelayCommand(OnAddStockItem);
            EditStockItemCommand = new RelayCommand(OnEditStockItem);
            DeleteStockItemCommand = new RelayCommand(OnDeleteStockItem);
            EditManufacturerCommand = new RelayCommand(OnManufacturers);
            EditStockArrivalCommand = new RelayCommand(OnStockArrival);
            EditExchangeRatesCommand = new RelayCommand(OnExchangeRates);
        }

        public void OnAddStockItem()
        {
            var vm = new StockItemViewModel(dbManufacturers, manufacturerRepo, null, "Visible", "Hidden");
            AddNewItemView addNewItemView = new AddNewItemView();
            addNewItemView.DataContext = vm;
            addNewItemView.Owner = App.Current.MainWindow;
            vm.Currency = Currency.UAH;
            vm.WindowTitle = "Добавить товарную позицию";

            addNewItemView.ShowDialog();

            if (addNewItemView.DialogResult == true)
            {
                var newItem = new StockItem(vm.Name, vm.Manufacturer, vm.Description, vm.Currency, vm.PurchasePrice.Value, vm.RetailPrice.Value);
                var item = stockItemRepo.Add(newItem);
                dbStockItems.Insert(0, item);
            }
        }
        public void OnEditStockItem()
        {
            var vm = new StockItemViewModel(dbManufacturers, manufacturerRepo, SelectedStockItem, "Hidden", "Visible");
            AddNewItemView editItemView = new AddNewItemView();

            editItemView.DataContext = vm;
            editItemView.Owner = App.Current.MainWindow;

            vm.Id = SelectedStockItem.Id;
            vm.Name = SelectedStockItem.Name;
            vm.Manufacturer = SelectedStockItem.Manufacturer;
            vm.Description = SelectedStockItem.Description;
            vm.Currency = SelectedStockItem.Currency;
            vm.PurchasePrice.Value = SelectedStockItem.PurchasePrice;
            vm.PurchasePrice.Input = SelectedStockItem.PurchasePrice.ToString();
            vm.RetailPrice.Value = SelectedStockItem.RetailPrice;
            vm.RetailPrice.Input = SelectedStockItem.RetailPrice.ToString();
            vm.Quantity = SelectedStockItem.Quantity;
            vm.WindowTitle = "Изменить товарную позицию";

            editItemView.ShowDialog();

            if (editItemView.DialogResult == true)
            {
                SelectedStockItem.Name = vm.Name;
                SelectedStockItem.Manufacturer = vm.Manufacturer;
                SelectedStockItem.Description = vm.Description;
                SelectedStockItem.Currency = vm.Currency;
                SelectedStockItem.PurchasePrice = vm.PurchasePrice.Value;
                SelectedStockItem.RetailPrice = vm.RetailPrice.Value;

                stockItemRepo.Update(SelectedStockItem);
            }
        }
        public void OnDeleteStockItem()
        {
            var result = stockItemRepo.TryDelete(SelectedStockItem);

            if (result == false)
            {
                MessageBox.Show($"Невозможно удалить {SelectedStockItem.Name}, т. к. данная позиция присутствует в заказах!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var userChoice = MessageBox.Show("Позиция: " + $"{SelectedStockItem.Name}.\nУдалить?", "Удаление складской позиции", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (userChoice == MessageBoxResult.Yes)
                {
                    stockItemRepo.Delete(SelectedStockItem);
                    dbStockItems.Remove(SelectedStockItem);
                }
            }
        }
        public void OnManufacturers()
        {
            var vm = new ManufacturerViewModel(dbManufacturers, manufacturerRepo);
            ManufacturerView manufacturerView = new ManufacturerView();
            manufacturerView.DataContext = vm;
            manufacturerView.Owner = App.Current.MainWindow;

            manufacturerView.ShowDialog();
        }
        private void OnExchangeRates()
        {
            var vm = new ExchangeRatesViewModel(dbExchangeRates, exchangeRateRepo);
            ExchangeRatesView exchangeRatesView = new ExchangeRatesView();

            exchangeRatesView.DataContext = vm;
            exchangeRatesView.Owner = App.Current.MainWindow;
            exchangeRatesView.ShowDialog();
        }
        public void OnStockArrival()
        {
            var vm = new StockArrivalViewModel(dbStockArrivals, stockArrivalRepo, stockItemRepo);
            StockArrivalView stockArrivalView = new StockArrivalView();

            stockArrivalView.DataContext = vm;
            stockArrivalView.Owner = App.Current.MainWindow;
            stockArrivalView.ShowDialog();
        }
    }
}
