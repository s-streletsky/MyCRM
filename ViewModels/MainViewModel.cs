using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private bool isClientEditButtonEnabled;
        private Client selectedClient;
        private Order selectedOrder;

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
        public Client SelectedClient {
            get { return selectedClient; }
            set { selectedClient = value;
                IsClientEditButtonEnabled = true; }
        }
        public StockItem SelectedStockItem { get; set; }
        public Order SelectedOrder { 
            get { return selectedOrder; } 
            set { selectedOrder = value; }
        }
        public Currency SelectedCurrency { get; set; }
        public bool IsClientEditButtonEnabled { 
            get { return isClientEditButtonEnabled; }
            set { isClientEditButtonEnabled = value;
                OnPropertyChanged(); }
        }

        public float CurrencyExchangeRate { get; set; }

        private ClientRepository clientRepo = new ClientRepository();
        private OrderRepository orderRepo = new OrderRepository();
        private OrderItemRepository orderItemRepo = new OrderItemRepository();
        private ExchangeRateRepository exchangeRateRepo = new ExchangeRateRepository();
        private StockItemRepository stockItemRepo = new StockItemRepository();
        private ManufacturerRepository manufacturerRepo = new ManufacturerRepository();
        private StockArrivalRepository stockArrivalRepo = new StockArrivalRepository();

        public MainViewModel()
        {
            AddClientCommand = new RelayCommand(OnAddClient);
            EditClientCommand = new RelayCommand(OnEditClient);
            DeleteClientCommand = new RelayCommand(OnDeleteClient);

            AddOrderCommand = new RelayCommand(OnAddOrder);
            EditOrderCommand = new RelayCommand(OnEditOrder);

            AddStockItemCommand = new RelayCommand(OnAddStockItem);
            EditStockItemCommand = new RelayCommand(OnEditStockItem);
            DeleteStockItemCommand = new RelayCommand(OnDeleteStockItem);

            EditManufacturerCommand = new RelayCommand(OnManufacturersClick);
            EditStockArrivalCommand = new RelayCommand(OnStockArrivalClick);
            EditExchangeRatesCommand = new RelayCommand(OnExchangeRatesClick);
            
            LoadClientsCommand = new RelayCommand(OnLoadClients_Click);
            LoadOrdersCommand = new RelayCommand(OnLoadOrders_Click);
            AddNewExchangeRateCommand = new RelayCommand(OnAddNewExchangeRate_Click);

            Database = new Database();
            SelectedClient = null;

            IsClientEditButtonEnabled = false;

            clientRepo.GetAll(Database);
            manufacturerRepo.GetAll(Database);
            stockItemRepo.GetAll(Database);
            orderRepo.GetAll(Database);
            orderItemRepo.GetAll(Database);
            exchangeRateRepo.GetAll(Database);            
            stockArrivalRepo.GetAll(Database);
        }
        // Кнопки во вкладке "Клиенты"
        public void OnAddClient()
        {
            var vm = new ClientViewModel(Database, null, clientRepo);
            AddNewClientView addNewClientView = new AddNewClientView();

            addNewClientView.DataContext = vm;
            addNewClientView.Show();
        }
        public void OnEditClient()
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
        public void OnDeleteClient()
        {
            var result = clientRepo.TryDelete(SelectedClient);

            if (result == false)
            {
                MessageBox.Show("Невозможно удалить клиента, если в его карточке присутствует хотя бы 1 заказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var userChoice = MessageBox.Show("Клиент: " + $"{SelectedClient.Name}.\nУдалить?", "Удаление карточки клиента", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                if (userChoice == MessageBoxResult.Yes)
                {
                    clientRepo.Delete(SelectedClient);
                    var i = Database.Clients.IndexOf(SelectedClient);
                    Database.Clients.RemoveAt(i);
                }
            }
        }

        //Кнопки во вкладке "Заказы"
        public void OnAddOrder()
        {
            var vm = new OrderViewModel(Database, exchangeRateRepo, orderItemRepo, SelectedOrder);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            orderView.ShowDialog();

            if (orderView.DialogResult == true)
            {
                var o = new Order(vm.Date, vm.Client, vm.Status);
                var order = orderRepo.Add(o);
                Database.Orders.Add(order);
            }
        }
        public void OnEditOrder()
        {
            if (SelectedOrder != null)
            {
                var vm = new OrderViewModel(Database, exchangeRateRepo, orderItemRepo, SelectedOrder);
                OrderView orderView = new OrderView();
                orderView.DataContext = vm;

                vm.Id = SelectedOrder.Id;
                vm.Client = SelectedOrder.Client;
                vm.Date = SelectedOrder.Created;
                vm.Status = SelectedOrder.Status;
                vm.IsDataGridEnabled = true;

                foreach (var item in Database.OrdersItems)
                {
                    if (item.Order.Id == SelectedOrder.Id)
                    {
                        vm.OrderItems.Add(item);
                        vm.UpdateBillingDetails();
                    }
                }

                orderView.ShowDialog();               
            }
        }

        // Кнопки во вкладке "Склад"
        public void OnAddStockItem()
        {
            var vm = new StockItemViewModel(Database, stockItemRepo, manufacturerRepo, null, "Visible", "Hidden");
            AddNewItemView addNewItemView = new AddNewItemView();
            addNewItemView.DataContext = vm;
            addNewItemView.Show();
        }
        public void OnEditStockItem()
        {
            if (SelectedStockItem != null)
            {
                var vm = new StockItemViewModel(Database, stockItemRepo, manufacturerRepo, SelectedStockItem, "Hidden", "Visible");
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
        public void OnDeleteStockItem()
        {
            stockItemRepo.Delete(SelectedStockItem);
            var i = Database.StockItems.IndexOf(SelectedStockItem);
            Database.StockItems.RemoveAt(i);
        }
        public void OnManufacturersClick()
        {
            var vm = new ManufacturerViewModel(Database, manufacturerRepo);
            ManufacturerView manufacturerView = new ManufacturerView();

            manufacturerView.DataContext = vm;
            manufacturerView.Show();
        }
        private void OnExchangeRatesClick()
        {
            var vm = new ExchangeRatesViewModel(Database, exchangeRateRepo);
            ExchangeRatesView exchangeRatesView = new ExchangeRatesView();

            exchangeRatesView.DataContext = vm;
            exchangeRatesView.Show();
        }
        public void OnStockArrivalClick()
        {
            var vm = new StockArrivalViewModel(Database, stockArrivalRepo, stockItemRepo);
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
            var newExRate = exchangeRateRepo.Add(new ExchangeRate(SelectedCurrency, CurrencyExchangeRate));
            Database.ExchangeRates.Insert(0, newExRate);
        }


    }
}
