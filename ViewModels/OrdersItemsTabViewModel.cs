using CRM.Models;
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
    internal class OrdersItemsTabViewModel : ViewModelBase
    {
        private ObservableCollection<OrderItem> dbOrdersItems;
        private ObservableCollection<StockItem> dbStockItems;
        private ObservableCollection<ExchangeRate> dbExchangeRates;
        private OrderItem selectedOrderItem;

        private OrderItemRepository orderItemRepo;
        private StockItemRepository stockItemRepo;
        private ExchangeRateRepository exchangeRateRepo;

        public RelayCommand EditOrderItemCommand { get; }
        public RelayCommand DeleteOrderItemCommand { get; }

        public ObservableCollection<OrderItem> DbOrdersItems { get { return dbOrdersItems; } }
        public OrderItem SelectedOrderItem
        {
            get { return selectedOrderItem; }
            set
            {
                selectedOrderItem = value;
                OnPropertyChanged(nameof(IsOrdersItemsButtonsEnabled));
            }
        }
        public bool IsOrdersItemsButtonsEnabled { get { return SelectedOrderItem != null; } }

        public OrdersItemsTabViewModel(ObservableCollection<OrderItem> oi, ObservableCollection<StockItem> si, ObservableCollection<ExchangeRate> er, OrderItemRepository oir, StockItemRepository sir, ExchangeRateRepository err)
        {
            dbOrdersItems = oi;
            dbStockItems = si;
            dbExchangeRates = er;

            orderItemRepo = oir;
            stockItemRepo = sir;
            exchangeRateRepo = err;

            EditOrderItemCommand = new RelayCommand(OnEditOrderItem);
            DeleteOrderItemCommand = new RelayCommand(OnDeleteOrderItem);
        }

        public void OnEditOrderItem()
        {
            var vm = new OrderItemViewModel(dbStockItems, dbExchangeRates, exchangeRateRepo);
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
                dbOrdersItems.Remove(SelectedOrderItem);
            }
        }
    }
}
