using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ClientsTabViewModel clientsTabViewModel;

        private Database db;

        private Client selectedClient;
        private Order selectedOrder;
        private StockItem selectedStockItem;
        private OrderItem selectedOrderItem;

        public ClientsTabViewModel ClientsTabViewModel { get { return clientsTabViewModel; } }

        //public RelayCommand AddClientCommand { get; }
        //public RelayCommand EditClientCommand { get; }
        //public RelayCommand DeleteClientCommand { get; }
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
        public RelayCommand EditOrderItemCommand { get; }
        public RelayCommand DeleteOrderItemCommand { get; }
        //public Client SelectedClient {
        //    get { return selectedClient; }
        //    set { selectedClient = value;
        //        OnPropertyChanged(nameof(IsClientsButtonsEnabled));
        //    }
        //}
        public StockItem SelectedStockItem { 
            get { return selectedStockItem; }
            set { selectedStockItem = value;
                OnPropertyChanged(nameof(IsStockButtonsEnabled));
            }
        }
        public Order SelectedOrder { 
            get { return selectedOrder; } 
            set { selectedOrder = value;
                OnPropertyChanged(nameof(IsOrdersButtonsEnabled)); }
        }
        public OrderItem SelectedOrderItem { 
            get { return selectedOrderItem; } 
            set { selectedOrderItem = value; 
                OnPropertyChanged(nameof(IsOrdersItemsButtonsEnabled)); } 
        }
        public Currency SelectedCurrency { get; set; }
        //public bool IsClientsButtonsEnabled { 
        //    get { return SelectedClient != null; }
        //}
        public bool IsOrdersButtonsEnabled { 
            get { return SelectedOrder != null; }
        }
        public bool IsStockButtonsEnabled { 
            get { return SelectedStockItem != null; }
        }
        public bool IsOrdersItemsButtonsEnabled { get { return SelectedOrderItem != null; } }
        public bool IsStatsEnabled { get { return db.Orders.Count != 0; } }

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
            //AddClientCommand = new RelayCommand(OnAddClient);
            //EditClientCommand = new RelayCommand(OnEditClient);
            //DeleteClientCommand = new RelayCommand(OnDeleteClient);

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

            EditOrderItemCommand = new RelayCommand(OnEditOrderItem);
            DeleteOrderItemCommand = new RelayCommand(OnDeleteOrderItem);

            db = new Database();           

            if (File.Exists("db.sqlite") == false)
            {
                using (FileStream fs = File.Create("db.sqlite")) { }
                db.CreateEmptyDatabase();
            }

            clientRepo.GetAll(db);
            manufacturerRepo.GetAll(db);
            stockItemRepo.GetAll(db);
            orderRepo.GetAll(db);
            paymentRepo.GetAll(db.Payments, db.Clients, db.Orders);
            orderItemRepo.GetAll(db);
            exchangeRateRepo.GetAll(db);
            stockArrivalRepo.GetAll(db);

            clientsTabViewModel = new ClientsTabViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, orderRepo, orderItemRepo, exchangeRateRepo, stockItemRepo, paymentRepo);
        }

        //Кнопки во вкладке "Клиенты"
        //public void OnAddClient()
        //{
        //    var vm = new ClientViewModel(Db, null, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
        //    AddNewClientView addNewClientView = new AddNewClientView();

        //    addNewClientView.DataContext = vm;
        //    addNewClientView.Owner = App.Current.MainWindow;
        //    vm.WindowTitle = "Добавить нового клиента";

        //    addNewClientView.ShowDialog();

        //    if (addNewClientView.DialogResult == true)
        //    {
        //        var newClient = new Client();
        //        newClient.Name = vm.Name;
        //        newClient.Nickname = vm.Nickname;
        //        newClient.Phone = vm.Phone;
        //        newClient.Email = vm.Email;
        //        newClient.Country = vm.Country;
        //        newClient.City = vm.City;
        //        newClient.Address = vm.Address;
        //        newClient.ShippingMethod = vm.ShippingMethod;
        //        newClient.PostalCode = vm.PostalCode;

        //        var client = clientRepo.Add(newClient);
        //        Db.Clients.Insert(0, client);
        //    }
        //}
        //public void OnEditClient()
        //{            
        //    if (SelectedClient != null)
        //    {
        //        var vm = new ClientViewModel(Db, SelectedClient, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
        //        AddNewClientView EditClientView = new AddNewClientView();

        //        EditClientView.DataContext = vm;
        //        EditClientView.Owner = App.Current.MainWindow;

        //        vm.Id = SelectedClient.Id;
        //        vm.Date = (DateTime)SelectedClient.Date;
        //        vm.Name = SelectedClient.Name;
        //        vm.Nickname = SelectedClient.Nickname;
        //        vm.Phone = SelectedClient.Phone;
        //        vm.Email = SelectedClient.Email;
        //        vm.Country = SelectedClient.Country;
        //        vm.City = SelectedClient.City;
        //        vm.Address = SelectedClient.Address;
        //        vm.ShippingMethod = SelectedClient.ShippingMethod;
        //        vm.PostalCode = SelectedClient.PostalCode;
        //        vm.Notes = SelectedClient.Notes;
        //        vm.Balance = clientRepo.GetBalance(SelectedClient);
        //        vm.IsDataGridEnabled = true;                
        //        vm.WindowTitle = "Изменить данные клиента";

        //        foreach (var order in Db.Orders)
        //        {
        //            if (SelectedClient.Id == order.Client.Id)
        //            {
        //                vm.Orders.Add(order);
        //            }
        //        }                

        //        EditClientView.ShowDialog();

        //        if (EditClientView.DialogResult == true)
        //        {
        //            SelectedClient.Name = vm.Name;
        //            SelectedClient.Nickname = vm.Nickname;
        //            SelectedClient.Phone = vm.Phone;
        //            SelectedClient.Email = vm.Email;
        //            SelectedClient.Country = vm.Country;
        //            SelectedClient.City = vm.City;
        //            SelectedClient.Address = vm.Address;
        //            SelectedClient.ShippingMethod = vm.ShippingMethod;
        //            SelectedClient.PostalCode = vm.PostalCode;
        //            SelectedClient.Notes = vm.Notes;

        //            clientRepo.Update(SelectedClient);
        //        }
        //    }
        //}
        //public void OnDeleteClient()
        //{
        //    var result = clientRepo.TryDelete(SelectedClient);

        //    if (result == false)
        //    {
        //        MessageBox.Show("Невозможно удалить клиента, если в его карточке присутствует хотя бы 1 заказ!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //    else
        //    {
        //        var userChoice = MessageBox.Show("Клиент: " + $"{SelectedClient.Name}.\nУдалить?", "Удаление карточки клиента", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

        //        if (userChoice == MessageBoxResult.Yes)
        //        {
        //            clientRepo.Delete(SelectedClient);
        //            Db.Clients.Remove(SelectedClient);
        //        }
        //    }
        //}

        //Кнопки во вкладке "Заказы"
        public void OnAddOrder()
        {
            var vm = new OrderViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            orderView.Owner = App.Current.MainWindow;
            vm.WindowTitle = "Добавить новый заказ";

            orderView.ShowDialog();

            if (orderView.DialogResult == true)
            {
                var o = new Order(vm.Date, vm.Client, vm.Status);
                var order = orderRepo.Add(o);
                db.Orders.Insert(0, order);
            }
        }
        public void OnEditOrder()
        {
            var vm = new OrderViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            orderView.Owner = App.Current.MainWindow;

            vm.Id = SelectedOrder.Id;
            vm.Client = SelectedOrder.Client;
            vm.Date = SelectedOrder.Date;
            vm.Status = SelectedOrder.Status;
            vm.Notes = SelectedOrder.Notes;
            vm.IsDataGridEnabled = true;
            vm.IsChooseClientButtonEnabled = false;
            vm.WindowTitle = "Изменить содержимое заказа";

            foreach (var item in db.OrdersItems)
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
        public void OnDeleteOrder()
        {
            var userChoice = MessageBox.Show("Заказ №" + $"{SelectedOrder.Id}\nУдалить?", "Удаление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            List<OrderItem> orderItems = new List<OrderItem>();
            List<Payment> orderPayments = new List<Payment>();

            if (userChoice == MessageBoxResult.Yes)
            {
                foreach (var item in db.OrdersItems)
                {
                    if (item.Order.Id == SelectedOrder.Id)
                    {
                        orderItemRepo.Delete(item);
                        stockItemRepo.UpdateQuantity(item.StockItem);
                        orderItems.Add(item);
                    }
                }

                foreach (var item in orderItems)
                {
                    db.OrdersItems.Remove(item);
                }

                foreach (var payment in db.Payments)
                {
                    if (payment.Order.Id == SelectedOrder.Id)
                    {
                        paymentRepo.Delete(payment);
                        orderPayments.Add(payment);
                    }
                }

                foreach (var payment in orderPayments)
                {
                    db.Payments.Remove(payment);
                }

                orderRepo.Delete(SelectedOrder);
                db.Orders.Remove(SelectedOrder);                
            }
        }
        public void OnPayments()
        {
            var vm = new PaymentsViewModel(db.Payments, paymentRepo);
            PaymentsView paymentsView = new PaymentsView();
            paymentsView.DataContext = vm;
            paymentsView.Owner = App.Current.MainWindow;
            paymentsView.ShowDialog();
        }
        public void OnStats()
        {
            var vm = new StatsViewModel(db);
            StatsView statsView = new StatsView();
            statsView.DataContext = vm;
            statsView.Owner = App.Current.MainWindow;
            vm.PrintStats();

            statsView.ShowDialog();
        }

        //Кнопки во вкладке "Склад"
        public void OnAddStockItem()
        {
            var vm = new StockItemViewModel(db, stockItemRepo, manufacturerRepo, null, "Visible", "Hidden");
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
                db.StockItems.Insert(0, item);
            }
        }
        public void OnEditStockItem()
        {
            var vm = new StockItemViewModel(db, stockItemRepo, manufacturerRepo, SelectedStockItem, "Hidden", "Visible");
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
                    db.StockItems.Remove(SelectedStockItem);
                }
            }
        }
        public void OnManufacturers()
        {
            var vm = new ManufacturerViewModel(db, manufacturerRepo);
            ManufacturerView manufacturerView = new ManufacturerView();
            manufacturerView.DataContext = vm;
            manufacturerView.Owner = App.Current.MainWindow;

            manufacturerView.ShowDialog();
        }
        private void OnExchangeRates()
        {
            var vm = new ExchangeRatesViewModel(db, exchangeRateRepo);
            ExchangeRatesView exchangeRatesView = new ExchangeRatesView();

            exchangeRatesView.DataContext = vm;
            exchangeRatesView.Owner = App.Current.MainWindow;
            exchangeRatesView.ShowDialog();
        }
        public void OnStockArrival()
        {
            var vm = new StockArrivalViewModel(db, stockArrivalRepo, stockItemRepo);
            StockArrivalView stockArrivalView = new StockArrivalView();

            stockArrivalView.DataContext = vm;
            stockArrivalView.Owner = App.Current.MainWindow;
            stockArrivalView.ShowDialog();
        }

        //Кнопки во вкладке "Позиции"
        public void OnEditOrderItem()
        {
            var vm = new OrderItemViewModel(db.StockItems, db.ExchangeRates, exchangeRateRepo);
            OrderItemView orderItemView = new OrderItemView();
            orderItemView.DataContext = vm;
            orderItemView.Owner = App.Current.MainWindow;

            vm.IsChooseStockItemButtonEnabled = false;
            vm.OrderId = SelectedOrderItem.Order.Id;
            vm.SelectedItem = SelectedOrderItem.StockItem;
            vm.PurchasePrice = SelectedOrderItem.StockItem.PurchasePrice * SelectedOrderItem.ExchangeRate;
            vm.RetailPrice = SelectedOrderItem.StockItem.RetailPrice * SelectedOrderItem.ExchangeRate;
            vm.Quantity.Value = SelectedOrderItem.Quantity;
            vm.Quantity.Input = SelectedOrderItem.Quantity.ToString();
            vm.Discount.Value = SelectedOrderItem.Discount;
            vm.Discount.Input = SelectedOrderItem.Discount.ToString();
            vm.ExchangeRate = SelectedOrderItem.ExchangeRate;
            vm.WindowTitle = "Изменить товарную позицию в заказе";
            vm.CalculateBillingInfo();

            orderItemView.ShowDialog();

            if (orderItemView.DialogResult == true)
            {
                SelectedOrderItem.Quantity = vm.Quantity.Value;
                SelectedOrderItem.Discount = vm.Discount.Value;
                SelectedOrderItem.Total = vm.Total;
                SelectedOrderItem.Expenses = vm.Expenses;
                SelectedOrderItem.Profit = vm.Profit;

                orderItemRepo.Update(SelectedOrderItem);
                stockItemRepo.UpdateQuantity(SelectedOrderItem.StockItem);
            }
        }
        public void OnDeleteOrderItem()
        {
            var userChoice = MessageBox.Show("Наименование: " + $"{SelectedOrderItem.StockItem.Name}.\nУдалить?", "Удаление позиции заказа", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (userChoice == MessageBoxResult.Yes)
            {
                orderItemRepo.Delete(SelectedOrderItem);
                stockItemRepo.UpdateQuantity(SelectedOrderItem.StockItem);
                db.OrdersItems.Remove(SelectedOrderItem);
            }
        }
    }
}
