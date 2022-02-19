using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.WPF;

namespace CRM.ViewModels
{
    class AddNewItemViewModel : ViewModelBase
    {
        public RelayCommand AddItemCommand { get; }

        public MainViewModel mainViewModel;
        private Repository repo = new Repository();

        string title;
        string currency;
        string notes;
        decimal purchasePrice;
        decimal retailPrice;
        double quantity;

        public List<string> CurrenciesList { get; set; } = new List<string>();
        public int Id { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();

            }
        }

        public string Currency { 
            get { return currency; }
            set { 
                currency = value;
                OnPropertyChanged();
            } 
        }
        public string Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }
        public decimal PurchasePrice
        {
            get { return purchasePrice; }
            set {
                purchasePrice = value;
                OnPropertyChanged();
            }
        }
        public decimal RetailPrice
        {
            get { return retailPrice; }
            set
            {
                retailPrice = value;
                OnPropertyChanged();
            }
        }
        public double Quantity
        {
            get { return quantity; }
            set {
                quantity = value;
                OnPropertyChanged();
            } 
        }

        public AddNewItemViewModel()
        {

        }

        public AddNewItemViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            AddItemCommand = new RelayCommand(OnAddItemButton_Click);

            foreach (var currency in mainViewModel.Database.Currencies)
            {
                CurrenciesList.Add(currency.Code);
            }
        }

        void OnAddItemButton_Click(object _)
        {
            var item = new StockItem(Id, Title, Currency, PurchasePrice, RetailPrice, Quantity);
            mainViewModel.Database.Stock.Add(item);

            Title = null;
            Currency = Currency;
            PurchasePrice = 0;
            Quantity = 0;
            Id = Id + 1;
        }
    }
}
