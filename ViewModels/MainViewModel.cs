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
        private bool isClientsButtonsEnabled;
        private bool isOrdersButtonsEnabled;
        private bool isStockButtonsEnabled;
        private Client selectedClient;
        private Order selectedOrder;
        private StockItem selectedStockItem;

        public RelayCommand AddClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand DeleteClientCommand { get; }
        public RelayCommand AddOrderCommand { get; }
        public RelayCommand EditOrderCommand { get; }
        public RelayCommand DeleteOrderCommand { get; }
        public RelayCommand PaymentsCommand { get; }
        public RelayCommand StatsCommand { get; }
        public RelayCommand AddNewExchangeRateCommand { get; }
        public RelayCommand AddStockItemCommand { get; }
        public RelayCommand EditStockItemCommand { get; }
        public RelayCommand DeleteStockItemCommand { get; }
        public RelayCommand EditManufacturerCommand { get; }
        public RelayCommand EditStockArrivalCommand { get; }
        public RelayCommand EditExchangeRatesCommand { get; }
        public Database Database { get; set; }
        public Client SelectedClient {
            get { return selectedClient; }
            set { selectedClient = value;
                if (value != null) IsClientsButtonsEnabled = true;
                else IsClientsButtonsEnabled = false; }
        }
        public StockItem SelectedStockItem { 
            get { return selectedStockItem; } 
            set { selectedStockItem = value; 
                OnPropertyChanged();
                if (value != null) IsStockButtonsEnabled = true;
                else IsStockButtonsEnabled = false; } 
        }
        public Order SelectedOrder { 
            get { return selectedOrder; } 
            set { selectedOrder = value;
                if (value != null) IsOrdersButtonsEnabled = true;
                else IsOrdersButtonsEnabled = false; }
        }
        public Currency SelectedCurrency { get; set; }
        public bool IsClientsButtonsEnabled { 
            get { return isClientsButtonsEnabled; }
            set { isClientsButtonsEnabled = value;
                OnPropertyChanged(); }
        }
        public bool IsOrdersButtonsEnabled { 
            get { return isOrdersButtonsEnabled; } 
            set { isOrdersButtonsEnabled = value; 
                OnPropertyChanged(); } 
        }
        public bool IsStockButtonsEnabled { 
            get { return isStockButtonsEnabled; } 
            set { isStockButtonsEnabled = value; 
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
        private PaymentRepository paymentRepo = new PaymentRepository();

        public MainViewModel()
        {
            AddClientCommand = new RelayCommand(OnAddClient);
            EditClientCommand = new RelayCommand(OnEditClient);
            DeleteClientCommand = new RelayCommand(OnDeleteClient);

            AddOrderCommand = new RelayCommand(OnAddOrder);
            EditOrderCommand = new RelayCommand(OnEditOrder);
            DeleteOrderCommand = new RelayCommand(OnDeleteOrder);
            PaymentsCommand = new RelayCommand(OnPayments);
            StatsCommand = new RelayCommand(OnStats);

            AddStockItemCommand = new RelayCommand(OnAddStockItem);
            EditStockItemCommand = new RelayCommand(OnEditStockItem);
            DeleteStockItemCommand = new RelayCommand(OnDeleteStockItem);

            EditManufacturerCommand = new RelayCommand(OnManufacturers);
            EditStockArrivalCommand = new RelayCommand(OnStockArrival);
            EditExchangeRatesCommand = new RelayCommand(OnExchangeRates);           

            Database = new Database();
            SelectedClient = null;

            //IsClientsButtonsEnabled = false;

            clientRepo.GetAll(Database);
            manufacturerRepo.GetAll(Database);
            stockItemRepo.GetAll(Database);
            orderRepo.GetAll(Database);
            paymentRepo.GetAll(Database.Payments, Database.Clients, Database.Orders);
            orderItemRepo.GetAll(Database);
            exchangeRateRepo.GetAll(Database);            
            stockArrivalRepo.GetAll(Database);
        }
        //Кнопки во вкладке "Клиенты"
        public void OnAddClient()
        {
            var vm = new ClientViewModel(Database, null, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
            AddNewClientView addNewClientView = new AddNewClientView();

            addNewClientView.DataContext = vm;
            vm.WindowTitle = "Добавить нового клиента";

            addNewClientView.ShowDialog();

            if (addNewClientView.DialogResult == true)
            {
                var newClient = new Client();
                newClient.Name = vm.Name;
                newClient.Nickname = vm.Nickname;
                newClient.Phone = vm.Phone;
                newClient.Email = vm.Email;
                newClient.Country = vm.Country;
                newClient.City = vm.City;
                newClient.Address = vm.Address;
                newClient.ShippingMethod = vm.ShippingMethod;
                newClient.PostalCode = vm.PostalCode;

                var client = clientRepo.Add(newClient);
                Database.Clients.Insert(0, client);
            }
        }
        public void OnEditClient()
        {            
            if (SelectedClient != null)
            {
                var vm = new ClientViewModel(Database, SelectedClient, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
                AddNewClientView addNewClientView = new AddNewClientView();

                addNewClientView.DataContext = vm;

                vm.Id = SelectedClient.Id;
                vm.Date = (DateTime)SelectedClient.Date;
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
                vm.Balance = clientRepo.GetBalance(SelectedClient);
                vm.IsDataGridEnabled = true;
                vm.WindowTitle = "Изменить данные клиента";

                foreach (var order in Database.Orders)
                {
                    if (SelectedClient.Id == order.Client.Id)
                    {
                        vm.Orders.Add(order);
                    }
                }                

                addNewClientView.ShowDialog();

                if (addNewClientView.DialogResult == true)
                {
                    SelectedClient.Name = vm.Name;
                    SelectedClient.Nickname = vm.Nickname;
                    SelectedClient.Phone = vm.Phone;
                    SelectedClient.Email = vm.Email;
                    SelectedClient.Country = vm.Country;
                    SelectedClient.City = vm.City;
                    SelectedClient.Address = vm.Address;
                    SelectedClient.ShippingMethod = vm.ShippingMethod;
                    SelectedClient.PostalCode = vm.PostalCode;
                    SelectedClient.Notes = vm.Notes;

                    clientRepo.Update(SelectedClient);
                }
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
                    Database.Clients.Remove(SelectedClient);
                }
            }
        }

        //Кнопки во вкладке "Заказы"
        public void OnAddOrder()
        {
            var vm = new OrderViewModel(Database, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            vm.WindowTitle = "Добавить новый заказ";

            orderView.ShowDialog();

            if (orderView.DialogResult == true)
            {
                var o = new Order(vm.Date, vm.Client, vm.Status);
                var order = orderRepo.Add(o);
                Database.Orders.Insert(0, order);
            }
        }
        public void OnEditOrder()
        {
            if (SelectedOrder != null)
            {
                var vm = new OrderViewModel(Database, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
                OrderView orderView = new OrderView();
                orderView.DataContext = vm;

                vm.Id = SelectedOrder.Id;
                vm.Client = SelectedOrder.Client;
                vm.Date = SelectedOrder.Date;
                vm.Status = SelectedOrder.Status;
                vm.Notes = SelectedOrder.Notes;
                vm.IsDataGridEnabled = true;
                vm.WindowTitle = "Изменить содержимое заказа";

                foreach (var item in Database.OrdersItems)
                {
                    if (item.Order.Id == SelectedOrder.Id)
                    {
                        vm.OrderItems.Add(item);
                    }
                }

                vm.UpdateBillingDetails();
                orderView.ShowDialog();

                if (orderView.DialogResult == true)
                {
                    SelectedOrder.Client = vm.Client;
                    SelectedOrder.Status = vm.Status;
                    SelectedOrder.Notes = vm.Notes;

                    orderRepo.Update(SelectedOrder);
                }
            }
        }
        public void OnDeleteOrder()
        {
            var userChoice = MessageBox.Show("Заказ №" + $"{SelectedOrder.Id}\nУдалить?", "Удаление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            List<OrderItem> orderItems = new List<OrderItem>();

            if (userChoice == MessageBoxResult.Yes)
            {
                foreach (var item in Database.OrdersItems)
                {
                    if (item.Order.Id == SelectedOrder.Id)
                    {
                        orderItemRepo.Delete(item);
                        orderItems.Add(item);
                    }
                }

                foreach (var item in orderItems)
                {
                    Database.OrdersItems.Remove(item);
                }

                orderRepo.Delete(SelectedOrder);
                Database.Orders.Remove(SelectedOrder);
            }
        }
        public void OnPayments()
        {
            var vm = new PaymentsViewModel(Database.Payments);
            PaymentsView paymentsView = new PaymentsView();
            paymentsView.DataContext = vm;
            paymentsView.ShowDialog();
        }
        public void OnStats()
        {
            var vm = new StatsViewModel(Database);
            StatsView statsView = new StatsView();
            statsView.DataContext = vm;
            vm.PrintStats();

            statsView.ShowDialog();
        }

        //Кнопки во вкладке "Склад"
        public void OnAddStockItem()
        {
            var vm = new StockItemViewModel(Database, stockItemRepo, manufacturerRepo, null, "Visible", "Hidden");
            AddNewItemView addNewItemView = new AddNewItemView();
            addNewItemView.DataContext = vm;
            vm.WindowTitle = "Добавить товарную позицию";

            addNewItemView.ShowDialog();

            if (addNewItemView.DialogResult == true)
            {
                var newItem = new StockItem(vm.Name, vm.Manufacturer, vm.Description, vm.Currency, vm.PurchasePrice, vm.RetailPrice);
                var item = stockItemRepo.Add(newItem);
                Database.StockItems.Insert(0, item);
            }
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
                vm.WindowTitle = "Изменить товарную позицию";

                addNewItemView.ShowDialog();

                if (addNewItemView.DialogResult == true)
                {
                    SelectedStockItem.Name = vm.Name;
                    SelectedStockItem.Manufacturer = vm.Manufacturer;
                    SelectedStockItem.Description = vm.Description;
                    SelectedStockItem.Currency = vm.Currency;
                    SelectedStockItem.PurchasePrice = vm.PurchasePrice;
                    SelectedStockItem.RetailPrice = vm.RetailPrice;

                    stockItemRepo.Update(SelectedStockItem);
                }
            }                       
        }
        public void OnDeleteStockItem()
        {
            stockItemRepo.Delete(SelectedStockItem);
            var i = Database.StockItems.IndexOf(SelectedStockItem);
            Database.StockItems.RemoveAt(i);
        }
        public void OnManufacturers()
        {
            var vm = new ManufacturerViewModel(Database, manufacturerRepo);
            ManufacturerView manufacturerView = new ManufacturerView();

            manufacturerView.DataContext = vm;
            manufacturerView.ShowDialog();
        }
        private void OnExchangeRates()
        {
            var vm = new ExchangeRatesViewModel(Database, exchangeRateRepo);
            ExchangeRatesView exchangeRatesView = new ExchangeRatesView();

            exchangeRatesView.DataContext = vm;
            exchangeRatesView.ShowDialog();
        }
        public void OnStockArrival()
        {
            var vm = new StockArrivalViewModel(Database, stockArrivalRepo, stockItemRepo);
            StockArrivalView stockArrivalView = new StockArrivalView();

            stockArrivalView.DataContext = vm;
            stockArrivalView.ShowDialog();
        }
    }
}
