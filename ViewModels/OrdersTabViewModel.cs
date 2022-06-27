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
    internal class OrdersTabViewModel : ViewModelBase
    {
        private ObservableCollection<Client> dbClients;
        private ObservableCollection<Order> dbOrders;
        private ObservableCollection<OrderItem> dbOrdersItems;
        private ObservableCollection<StockItem> dbStockItems;
        private ObservableCollection<ExchangeRate> dbExchangeRates;
        private ObservableCollection<Payment> dbPayments;
        private Order selectedOrder;

        private ClientRepository clientRepo;
        private OrderRepository orderRepo;
        private OrderItemRepository orderItemRepo;
        private ExchangeRateRepository exchangeRateRepo;
        private StockItemRepository stockItemRepo;
        private PaymentRepository paymentRepo;

        public RelayCommand AddOrderCommand { get; }
        public RelayCommand EditOrderCommand { get; }
        public RelayCommand DeleteOrderCommand { get; }
        public RelayCommand PaymentsCommand { get; }
        public RelayCommand StatsCommand { get; }

        public ObservableCollection<Order> DbOrders { get { return dbOrders; } }
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged(nameof(IsOrdersButtonsEnabled));
            }
        }
        public bool IsOrdersButtonsEnabled
        {
            get { return SelectedOrder != null; }
        }

        public OrdersTabViewModel(ObservableCollection<Client> c, ObservableCollection<Order> o, ObservableCollection<OrderItem> oi, ObservableCollection<StockItem> si, ObservableCollection<ExchangeRate> er, ObservableCollection<Payment> p, ClientRepository cr, OrderRepository or, OrderItemRepository oir, ExchangeRateRepository err, StockItemRepository sir, PaymentRepository pr)
        {
            dbClients = c;
            dbOrders = o;
            dbOrdersItems = oi;
            dbStockItems = si;
            dbExchangeRates = er;
            dbPayments = p;

            clientRepo = cr;
            orderRepo = or;
            orderItemRepo = oir;
            exchangeRateRepo = err;
            stockItemRepo = sir;
            paymentRepo = pr;

            AddOrderCommand = new RelayCommand(OnAddOrder);
            EditOrderCommand = new RelayCommand(OnEditOrder);
            DeleteOrderCommand = new RelayCommand(OnDeleteOrder);
            PaymentsCommand = new RelayCommand(OnPayments);
            StatsCommand = new RelayCommand(OnStats);
        }

        public void OnAddOrder()
        {
            var vm = new OrderViewModel(dbClients, dbOrders, dbOrdersItems, dbStockItems, dbExchangeRates, dbPayments, clientRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
            OrderView orderView = new OrderView();
            orderView.DataContext = vm;
            orderView.Owner = App.Current.MainWindow;
            vm.WindowTitle = "Добавить новый заказ";

            orderView.ShowDialog();

            if (orderView.DialogResult == true)
            {
                var o = new Order(vm.Date, vm.Client, vm.Status);
                var order = orderRepo.Add(o);
                dbOrders.Insert(0, order);
            }
        }
        public void OnEditOrder()
        {
            var vm = new OrderViewModel(dbClients, dbOrders, dbOrdersItems, dbStockItems, dbExchangeRates, dbPayments, clientRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo, SelectedOrder);
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

            foreach (var item in dbOrdersItems)
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
                foreach (var item in dbOrdersItems)
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
                    dbOrdersItems.Remove(item);
                }

                foreach (var payment in dbPayments)
                {
                    if (payment.Order.Id == SelectedOrder.Id)
                    {
                        paymentRepo.Delete(payment);
                        orderPayments.Add(payment);
                    }
                }

                foreach (var payment in orderPayments)
                {
                    dbPayments.Remove(payment);
                }

                orderRepo.Delete(SelectedOrder);
                dbOrders.Remove(SelectedOrder);
            }
        }
        public void OnPayments()
        {
            var vm = new PaymentsViewModel(dbPayments, paymentRepo);
            PaymentsView paymentsView = new PaymentsView();
            paymentsView.DataContext = vm;
            paymentsView.Owner = App.Current.MainWindow;
            paymentsView.ShowDialog();
        }
        public void OnStats()
        {
            var vm = new StatsViewModel(dbOrders, dbPayments);
            StatsView statsView = new StatsView();
            statsView.DataContext = vm;
            statsView.Owner = App.Current.MainWindow;
            vm.PrintStats();

            statsView.ShowDialog();
        }
    }
}
