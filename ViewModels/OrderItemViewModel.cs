using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.ViewModels
{
    internal class OrderItemViewModel : ViewModelBase
    {
        private int orderId;
        private float retailPrice;
        private float? purchasePrice;
        private BindableFloat quantity;
        private BindableFloat discount;
        private float total;
        private float expenses;
        private float profit;
        private float exchangeRate;

        private StockItem selectedItem;
        private ObservableCollection<StockItem> items;
        private ObservableCollection<ExchangeRate> exchangeRates;
        private ExchangeRateRepository exchangeRateRepo;

        public int OrderId {
            get { return orderId; }
            set { orderId = value;
                OnPropertyChanged(); }
        }
        public float RetailPrice {
            get { return retailPrice; } 
            set { retailPrice = value; 
                OnPropertyChanged(); } 
        }
        public float? PurchasePrice { 
            get { return purchasePrice; } 
            set { purchasePrice = value; 
                OnPropertyChanged(); } 
        } 
        public BindableFloat Quantity { 
            get { return quantity; } 
            set { quantity = value; 
                OnPropertyChanged(); }
        }
        public BindableFloat Discount { 
            get { return discount; }
            set { discount = value; 
                OnPropertyChanged(); } 
        }
        public float Total { 
            get { return total; } 
            private set { total = value; 
                OnPropertyChanged(); } 
        }
        public float Expenses { 
            get { return expenses; } 
            private set { expenses = value; 
                OnPropertyChanged(); } 
        }
        public float Profit { 
            get { return profit; } 
            private set { profit = value; 
                OnPropertyChanged(); } 
        }
        public float ExchangeRate { 
            get { return exchangeRate; } 
            set { exchangeRate = value; 
                OnPropertyChanged(); } 
        }
        public bool IsChooseStockItemButtonEnabled { get; set; }
        public string WindowTitle { get; set; }

        public RelayCommand ChooseStockItemCommand { get; }
        public RelayCommand TextChangedCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
        public StockItem SelectedItem {
            get { return selectedItem; }
            set { selectedItem = value;
                OnPropertyChanged(); }          
        }

        public OrderItemViewModel() { }
        public OrderItemViewModel(ObservableCollection<StockItem> i, ObservableCollection<ExchangeRate> er, ExchangeRateRepository err)
        {
            items = i;
            exchangeRates = er;
            exchangeRateRepo = err;

            IsChooseStockItemButtonEnabled = true;

            Quantity = new BindableFloat();
            Discount = new BindableFloat();

            ChooseStockItemCommand = new RelayCommand(OnChooseStockItem);
            TextChangedCommand = new RelayCommand(CalculateBillingInfo);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void OnChooseStockItem()
        {
            var vm = new ChooseStockItemViewModel(items);
            ChooseStockItemView chooseStockItemView = new ChooseStockItemView();
            chooseStockItemView.DataContext = vm;
            chooseStockItemView.Owner = App.Current.MainWindow;
            chooseStockItemView.ShowDialog();

            if (chooseStockItemView.DialogResult == true)
            {
                SelectedItem = vm.SelectedItem;

                exchangeRate = GetExchangeRate(SelectedItem.Currency);
                
                RetailPrice = SelectedItem.RetailPrice * exchangeRate;
                PurchasePrice = SelectedItem.PurchasePrice * exchangeRate;

                CalculateBillingInfo();
            }
        }
        internal void CalculateBillingInfo()
        {
            if (RetailPrice != null)
            {
                Total = Quantity.Value * (RetailPrice - (RetailPrice * (discount.Value / 100)));
            }
            else { Total = 0; }

            if (PurchasePrice != null)
            {
                Expenses = Quantity.Value * PurchasePrice.Value;
            }
            else { Expenses = 0; }

            Profit = Total - Expenses;
        }
        private float GetExchangeRate(Currency currency)
        {
            var exchangeRate = exchangeRates.First(x => x.Currency == currency);
            return exchangeRate.Value;
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
