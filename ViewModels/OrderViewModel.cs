using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;
using CRM.Models;
using System.Collections.ObjectModel;
using CRM.Views;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    internal class OrderViewModel : ViewModelBase
    {
        private int id;
        private Client client;
        private DateTime? date;
        private OrderStatus status;
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        private ExchangeRateRepository exchangeRateRepo;
        private OrderItemRepository orderItemRepo;
        private Order selectedOrder;

        private float total;
        private float paid;
        private float debt;
        private float expenses;
        private float profit;

        public RelayCommand ChooseClientCommand { get; }
        public RelayCommand AddStockItemCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
        public int Id { get; set; }
        public Client Client { 
            get { return client; } 
            set { client = value;
                OnPropertyChanged(); }
        }
        public DateTime? Date { 
            get { return date; }
            set { date = value;}
        }
        public OrderStatus Status { 
            get { return status; } 
            set { status = value; 
                OnPropertyChanged(); } 
        }
        public Database Database { get; set; }
        public bool IsDataGridEnabled { get; set; } = false;
        public ObservableCollection<OrderItem> OrderItems { get { return orderItems; } }


        public float Total { 
            get { return total; } 
            private set { total = value; 
                OnPropertyChanged(); } 
        }
        public float Paid { 
            get { return paid; } 
            private set { paid = value; 
                OnPropertyChanged(); } 
        }
        public float Debt { 
            get { return debt; } 
            private set { debt = value; 
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

        public OrderViewModel() { }

        public OrderViewModel(Database db, ExchangeRateRepository err, OrderItemRepository oir, Order selected)
        {
            Date = DateTime.Now;
            Status = OrderStatus.NEW;
            Database = db;
            selectedOrder = selected;

            exchangeRateRepo = err;
            orderItemRepo = oir;

            ChooseClientCommand = new RelayCommand(OnChooseClient);
            AddStockItemCommand = new RelayCommand(OnAddItem);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }
        private void OnChooseClient()
        {
            var vm = new ChooseClientViewModel(Database.Clients);
            ChooseClientView chooseClientView = new ChooseClientView();
            chooseClientView.DataContext = vm;
            chooseClientView.ShowDialog();

            if (chooseClientView.DialogResult == true)
            {
                Client = vm.SelectedClient;
            }
        }
        private void OnAddItem()
        {
            var vm = new OrderItemViewModel(Database.StockItems, Database.ExchangeRates, exchangeRateRepo);
            OrderItemView orderItemView = new OrderItemView();
            orderItemView.DataContext = vm;
            vm.OrderId = selectedOrder.Id;
            orderItemView.ShowDialog();

            if (orderItemView.DialogResult == true)
            {
                var newOrderItem = new OrderItem(-1, selectedOrder, vm.SelectedItem, vm.Quantity, vm.RetailPrice, vm.Discount, vm.Total, vm.Profit, vm.Expenses, vm.ExchangeRate);
                var orderItem = orderItemRepo.Add(newOrderItem);
                Database.OrdersItems.Add(orderItem);
                orderItems.Add(orderItem);
                UpdateBillingDetails();
            }
        }

        public void UpdateBillingDetails()
        {
            Total = Expenses = Profit = 0;

            foreach (var item in OrderItems)
            {
                Total += item.Total;
                Expenses += item.Expenses;
                Profit += item.Profit;
            }
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
