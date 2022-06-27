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

        public ClientsTabViewModel ClientsTabViewModel { get; }
        public OrdersTabViewModel OrdersTabViewModel { get; }
        public StockTabViewModel StockTabViewModel { get; }
        public OrdersItemsTabViewModel OrdersItemsTabViewModel { get; }

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

            ClientsTabViewModel = new ClientsTabViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, orderRepo, orderItemRepo, exchangeRateRepo, stockItemRepo, paymentRepo);
            OrdersTabViewModel = new OrdersTabViewModel(db.Clients, db.Orders, db.OrdersItems, db.StockItems, db.ExchangeRates, db.Payments, clientRepo, orderRepo, orderItemRepo, exchangeRateRepo, stockItemRepo, paymentRepo);
            StockTabViewModel = new StockTabViewModel(db.StockItems, db.StockArrivals, db.Manufacturers, db.ExchangeRates, stockItemRepo, stockArrivalRepo, manufacturerRepo, exchangeRateRepo);
            OrdersItemsTabViewModel = new OrdersItemsTabViewModel(db.OrdersItems, db.StockItems, db.ExchangeRates, orderItemRepo, stockItemRepo, exchangeRateRepo);
        }
    }
}
