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
        private Database db;

        private ClientsTabViewModel clientsTabViewModel;
        private OrdersTabViewModel ordersTabViewModel;
        private StockTabViewModel stockTabViewModel;
        private OrdersItemsTabViewModel ordersItemsTabViewModel;

        public ClientsTabViewModel ClientsTabViewModel { get { return clientsTabViewModel; } }
        public OrdersTabViewModel OrdersTabViewModel { get { return ordersTabViewModel; } }
        public StockTabViewModel StockTabViewModel { get { return stockTabViewModel; } }
        public OrdersItemsTabViewModel OrdersItemsTabViewModel { get { return ordersItemsTabViewModel; } }

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
            db = new Database();           

            if (File.Exists("db.sqlite") == false)
            {
                using (FileStream fs = File.Create("db.sqlite")) { }
                db.CreateEmptyDatabase();
            }

            clientRepo.GetAll(db.Clients);
            manufacturerRepo.GetAll(db.Manufacturers);
            stockItemRepo.GetAll(db.StockItems, db.Manufacturers);
            orderRepo.GetAll(db.Orders, db.Clients);
            paymentRepo.GetAll(db.Payments, db.Clients, db.Orders);
            orderItemRepo.GetAll(db.OrdersItems, db.Orders, db.StockItems);
            exchangeRateRepo.GetAll(db.ExchangeRates);
            stockArrivalRepo.GetAll(db.StockArrivals, db.StockItems);

            clientsTabViewModel = new ClientsTabViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, orderRepo, orderItemRepo, exchangeRateRepo, stockItemRepo, paymentRepo);
            ordersTabViewModel = new OrdersTabViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, orderRepo, orderItemRepo, exchangeRateRepo, stockItemRepo, paymentRepo);
            stockTabViewModel = new StockTabViewModel(db.StockItems, db.StockArrivals, db.Manufacturers, db.ExchangeRates, stockItemRepo, stockArrivalRepo, manufacturerRepo, exchangeRateRepo);
            ordersItemsTabViewModel = new OrdersItemsTabViewModel(db.OrdersItems, db.StockItems, db.ExchangeRates, orderItemRepo, stockItemRepo, exchangeRateRepo);
        }
    }
}
