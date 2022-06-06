﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;
using CRM.Models;
using System.Collections.ObjectModel;
using CRM.Views;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows;

namespace CRM.ViewModels
{
    internal class OrderViewModel : ViewModelBase
    {
        private Client client;
        private DateTime date;
        private OrderStatus status;
        private string notes;
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();
        private ExchangeRateRepository exchangeRateRepo;
        private OrderItemRepository orderItemRepo;
        private StockItemRepository stockItemRepo;
        private PaymentRepository paymentRepo;
        private Order selectedOrder;
        private OrderItem selectedItem;

        private List<Payment> payments;

        private float total;
        private float paid;
        private float debt;
        private float expenses;
        private float profit;

        private bool isButtonEnabled;

        public RelayCommand ChooseClientCommand { get; }
        public RelayCommand AddOrderItemCommand { get; }
        public RelayCommand EditOrderItemCommand { get; }
        public RelayCommand DeleteOrderItemCommand { get; }
        public RelayCommand AddPaymentCommand { get; }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
        public int Id { get; set; }
        public Client Client { 
            get { return client; } 
            set { client = value;
                OnPropertyChanged(); }
        }
        public DateTime Date { 
            get { return date; }
            set { date = value;}
        }
        public OrderStatus Status { 
            get { return status; } 
            set { status = value; 
                OnPropertyChanged(); } 
        }
        public string Notes { 
            get { return notes; } 
            set { notes = value; 
                OnPropertyChanged(); } 
        }
        public Database Database { get; set; }
        public bool IsDataGridEnabled { get; set; }
        public bool IsButtonEnabled { 
            get { return isButtonEnabled; } 
            set { isButtonEnabled = value; 
                OnPropertyChanged(); } 
        }
        public bool IsChooseClientButtonEnabled { get; set; }
        public ObservableCollection<OrderItem> OrderItems { get { return orderItems; } }
        public OrderItem SelectedItem { 
            get { return selectedItem; } 
            set { selectedItem = value;
                if (value != null) IsButtonEnabled = true; 
                else IsButtonEnabled = false; } 
        }


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

        public OrderViewModel(Database db, ExchangeRateRepository err, OrderItemRepository oir, StockItemRepository sir, PaymentRepository pr, Order selected)
        {
            Date = DateTime.Now;
            Status = OrderStatus.NEW;
            Database = db;
            selectedOrder = selected;

            exchangeRateRepo = err;
            orderItemRepo = oir;
            stockItemRepo = sir;
            paymentRepo = pr;

            IsDataGridEnabled = false;
            IsButtonEnabled = false;
            IsChooseClientButtonEnabled = true;

            payments = new List<Payment>();
            paymentRepo.GetOrderPayments(Database, payments, selectedOrder);

            ChooseClientCommand = new RelayCommand(OnChooseClient);
            AddOrderItemCommand = new RelayCommand(OnAddOrderItem);
            EditOrderItemCommand = new RelayCommand(OnEditOrderItem);
            DeleteOrderItemCommand = new RelayCommand(OnDeleteOrderItem);
            AddPaymentCommand = new RelayCommand(OnAddPayment);
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

        private void OnAddOrderItem()
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
                stockItemRepo.UpdateQuantity(orderItem.StockItem);
                Database.OrdersItems.Add(orderItem);
                orderItems.Add(orderItem);
                UpdateBillingDetails();
            }
        }
        private void OnEditOrderItem()
        {
            var vm = new OrderItemViewModel(Database.StockItems, Database.ExchangeRates, exchangeRateRepo);
            OrderItemView orderItemView = new OrderItemView();
            orderItemView.DataContext = vm;

            vm.IsChooseStockItemButtonEnabled = false;
            vm.OrderId = SelectedItem.Order.Id;
            vm.SelectedItem = SelectedItem.StockItem;
            vm.PurchasePrice = SelectedItem.StockItem.PurchasePrice * SelectedItem.ExchangeRate;
            vm.RetailPrice = SelectedItem.StockItem.RetailPrice * SelectedItem.ExchangeRate;
            vm.Quantity = SelectedItem.Quantity;
            vm.Discount = SelectedItem.Discount;
            vm.ExchangeRate = SelectedItem.ExchangeRate;
            vm.CalculateBillingInfo();

            orderItemView.ShowDialog();

            if (orderItemView.DialogResult == true)
            {
                SelectedItem.Quantity = vm.Quantity;
                SelectedItem.Discount = vm.Discount;
                SelectedItem.Total = vm.Total;
                SelectedItem.Expenses = vm.Expenses;
                SelectedItem.Profit = vm.Profit;

                orderItemRepo.Update(SelectedItem);
                stockItemRepo.UpdateQuantity(SelectedItem.StockItem);
                UpdateBillingDetails();
            }
        }
        private void OnDeleteOrderItem()
        {
            var userChoice = MessageBox.Show("Наименование: " + $"{SelectedItem.StockItem.Name}.\nУдалить?", "Удаление позиции заказа", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (userChoice == MessageBoxResult.Yes)
            {
                orderItemRepo.Delete(SelectedItem);
                stockItemRepo.UpdateQuantity(SelectedItem.StockItem);
                Database.OrdersItems.Remove(selectedItem);
                orderItems.Remove(SelectedItem);
                UpdateBillingDetails();
            }
        }

        private void OnAddPayment()
        {
            var vm = new OrderPaymentViewModel(selectedOrder.Client, selectedOrder, selectedOrder.Total);
            PaymentView paymentView = new PaymentView();
            paymentView.DataContext = vm;
            paymentView.ShowDialog();

            if (paymentView.DialogResult == true)
            {
                var newPayment = new Payment(-1, DateTime.Now, vm.Client, vm.Order, vm.Amount, vm.Notes);
                var payment = paymentRepo.Add(newPayment);
                Database.Payments.Insert(0, payment);
                payments.Clear();
                paymentRepo.GetOrderPayments(Database, payments, selectedOrder);
                UpdateBillingDetails();
            }
        }

        public void UpdateBillingDetails()
        {
            Total = Paid = Debt = Expenses = Profit = 0;

            foreach (var item in OrderItems)
            {
                Total += item.Total;
                Expenses += item.Expenses;
                Profit += item.Profit;
            }

            selectedOrder.Total = Total;

            foreach (var payment in payments)
            {
                Paid += payment.Amount;
            }

            Debt = Total - Paid;
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