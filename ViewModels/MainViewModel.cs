using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private decimal currencyExchangeRate;

        public RelayCommand AddClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand DeleteClientCommand { get; }
        public RelayCommand AddOrderCommand { get; }
        public RelayCommand EditOrderCommand { get; }
        public RelayCommand AddNewExchangeRateCommand { get; }       
        public RelayCommand AddStockItemCommand { get; }
        public RelayCommand EditStockItemCommand { get; }
        public RelayCommand DeleteStockItemCommand { get; }
        public RelayCommand EditManufacturerCommand { get; }
        public RelayCommand EditStockArrivalCommand { get; }
        public RelayCommand EditExchangeRatesCommand { get; }
        public RelayCommand LoadClientsCommand { get; }
        public RelayCommand LoadOrdersCommand { get; }
        public Database Database { get; set; }
        public Client SelectedClient { get; set; }
        public StockItem SelectedStockItem { get; set; }
        public Order SelectedOrder { get; set; }
        public Currency SelectedCurrency { get; set; }

        public float CurrencyExchangeRate { get; set; }

        private ClientRepository clientRepo = new ClientRepository();
        private OrderRepository orderRepo = new OrderRepository();
        private ExchangeRateRepository exRateRepo = new ExchangeRateRepository();
        private StockItemRepository stockRepo = new StockItemRepository();
        private ManufacturerRepository mfRepo = new ManufacturerRepository();
        private StockArrivalRepository stockArrivalRepo = new StockArrivalRepository();

        public MainViewModel()
        {
            AddClientCommand = new RelayCommand(OnAddClientClick);
            EditClientCommand = new RelayCommand(OnEditClientClick);
            DeleteClientCommand = new RelayCommand(OnDeleteClientClick);

            AddOrderCommand = new RelayCommand(OnAddOrderClick);
            EditOrderCommand = new RelayCommand(OnEditOrderClick);

            AddStockItemCommand = new RelayCommand(OnAddStockItemClick);
            EditStockItemCommand = new RelayCommand(OnEditStockItemClick);
            DeleteStockItemCommand = new RelayCommand(OnDeleteStockItemClick);

            EditManufacturerCommand = new RelayCommand(OnManufacturersClick);
            EditStockArrivalCommand = new RelayCommand(OnStockArrivalClick);
            EditExchangeRatesCommand = new RelayCommand(OnExchangeRatesClick);
            
            LoadClientsCommand = new RelayCommand(OnLoadClients_Click);
            LoadOrdersCommand = new RelayCommand(OnLoadOrders_Click);
            AddNewExchangeRateCommand = new RelayCommand(OnAddNewExchangeRate_Click);

            Database = new Database();
            SelectedClient = null;

            clientRepo.GetAll(Database);
            mfRepo.GetAll(Database);
            orderRepo.GetAll(Database);
            exRateRepo.GetAll(Database);
            stockRepo.GetAll(Database);
            stockArrivalRepo.GetAll(Database);
        }
        // Кнопки во вкладке "Клиенты"
        public void OnAddClientClick()
        {
            var vm = new ClientViewModel(Database, null, clientRepo);
            AddNewClientView addNewClientView = new AddNewClientView();

            addNewClientView.DataContext = vm;
            addNewClientView.Show();
        }

        public void OnEditClientClick()
        {            
            if (SelectedClient != null)
            {
                var vm = new ClientViewModel(Database, SelectedClient, clientRepo);
                AddNewClientView addNewClientView = new AddNewClientView();

                addNewClientView.DataContext = vm;

                vm.Id = SelectedClient.Id;
                vm.Created = (DateTime)SelectedClient.Created;
                vm.Name = SelectedClient.Name;
                vm.Nickname = SelectedClient.Nickname;
                vm.Phone = SelectedClient.Phone;
                vm.Email = SelectedClient.Email;
                vm.Country = SelectedClient.Country;
                vm.City = SelectedClient.City;
                vm.Address = SelectedClient.Address;
                vm.ShippingMethod = SelectedClient.ShippingMethod;
                vm.PostalCode = SelectedClient.PostalCode;
                vm.Notes = SelectedClient.Notes;

                foreach (var order in Database.Orders)
                {
                    if (SelectedClient.Id == order.Client.Id)
                    {
                        vm.Orders.Add(order);
                    }
                }

                addNewClientView.Show();
            }
        }

        public void OnDeleteClientClick()
        {
            clientRepo.Delete(SelectedClient);
            var i = Database.Clients.IndexOf(SelectedClient);
            Database.Clients.RemoveAt(i);
        }

        //Кнопки во вкладке "Заказы"
        public void OnAddOrderClick()
        {
            var vm = new OrderViewModel(Database);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            orderView.Show();
        }
        public void OnEditOrderClick()
        {
            if (SelectedOrder != null)
            {
                var vm = new OrderViewModel();
                OrderView orderView = new OrderView();

                orderView.DataContext = vm;

                //vm.

                orderView.Show();
            }
        }

        // Кнопки во вкладке "Склад"
        public void OnAddStockItemClick()
        {
            var vm = new StockItemViewModel(Database, stockRepo, mfRepo, null, "Visible", "Hidden");
            AddNewItemView addNewItemView = new AddNewItemView();
            addNewItemView.DataContext = vm;
            addNewItemView.Show();
        }
        public void OnEditStockItemClick()
        {
            if (SelectedStockItem != null)
            {
                var vm = new StockItemViewModel(Database, stockRepo, mfRepo, SelectedStockItem, "Hidden", "Visible");
                AddNewItemView addNewItemView = new AddNewItemView();

                addNewItemView.DataContext = vm;

                vm.Id = SelectedStockItem.Id;
                vm.Name = SelectedStockItem.Name;
                vm.Manufacturer = SelectedStockItem.Manufacturer;
                vm.Description = SelectedStockItem.Description;
                vm.Currency = SelectedStockItem.Currency;
                vm.PurchasePrice = SelectedStockItem.PurchasePrice;
                vm.RetailPrice = SelectedStockItem.RetailPrice;
                vm.Quantity = SelectedStockItem.Quantity;

                addNewItemView.Show();
            }                       
        }
        public void OnDeleteStockItemClick()
        {
            stockRepo.Delete(SelectedStockItem);
            var i = Database.StockItems.IndexOf(SelectedStockItem);
            Database.StockItems.RemoveAt(i);
        }
        public void OnManufacturersClick()
        {
            var vm = new ManufacturerViewModel(Database, mfRepo);
            ManufacturerView manufacturerView = new ManufacturerView();

            manufacturerView.DataContext = vm;
            manufacturerView.Show();
        }
        private void OnExchangeRatesClick()
        {
            var vm = new ExchangeRatesViewModel(Database, exRateRepo);
            ExchangeRatesView exchangeRatesView = new ExchangeRatesView();

            exchangeRatesView.DataContext = vm;
            exchangeRatesView.Show();
        }

        public void OnStockArrivalClick()
        {
            var vm = new StockArrivalViewModel(Database, stockArrivalRepo, stockRepo);
            StockArrivalView stockArrivalView = new StockArrivalView();

            stockArrivalView.DataContext = vm;
            stockArrivalView.Show();
        }

        public void OnLoadClients_Click()
        {
            clientRepo.GetAll(Database);
        }

        public void OnLoadOrders_Click()
        {
            orderRepo.GetAll(Database);
        }

        public void OnAddNewExchangeRate_Click()
        {
            var newExRate = exRateRepo.Add(new ExchangeRate(SelectedCurrency, CurrencyExchangeRate));
            Database.ExchangeRates.Insert(0, newExRate);
        }


    }
}
