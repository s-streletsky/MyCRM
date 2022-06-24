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
    internal class ClientsTabViewModel : ViewModelBase
    {
        private ObservableCollection<Client> dbClients;
        private ObservableCollection<Order> dbOrders;
        private ObservableCollection<OrderItem> dbOrdersItems;
        private ObservableCollection<StockItem> dbStockItems;
        private ObservableCollection<ExchangeRate> dbExchangeRates;
        private ObservableCollection<Payment> dbPayments;
        private Client selectedClient;

        private ClientRepository clientRepo;
        private OrderRepository orderRepo;
        private OrderItemRepository orderItemRepo;
        private ExchangeRateRepository exchangeRateRepo;
        private StockItemRepository stockItemRepo;
        private PaymentRepository paymentRepo;

        public RelayCommand AddClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand DeleteClientCommand { get; }

        public ObservableCollection<Client> DbClients { get { return dbClients; } }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set { selectedClient = value;
                OnPropertyChanged(nameof(IsClientsButtonsEnabled));
            }
        }
        public bool IsClientsButtonsEnabled
        {
            get { return SelectedClient != null; }
        }

        public ClientsTabViewModel(ObservableCollection<Client> c, ObservableCollection<Order> o, ObservableCollection<OrderItem> oi, ObservableCollection<StockItem> si, ObservableCollection<ExchangeRate> er, ObservableCollection<Payment> p, ClientRepository cr, OrderRepository or, OrderItemRepository oir, ExchangeRateRepository err, StockItemRepository sir, PaymentRepository pr)
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

            AddClientCommand = new RelayCommand(OnAddClient);
            EditClientCommand = new RelayCommand(OnEditClient);
            DeleteClientCommand = new RelayCommand(OnDeleteClient);
        }

        public void OnAddClient()
        {
            var vm = new ClientViewModel(dbClients, dbOrders, dbOrdersItems, dbStockItems, dbExchangeRates, dbPayments, null, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
            ClientView addClientView = new ClientView();

            addClientView.DataContext = vm;
            addClientView.Owner = App.Current.MainWindow;
            vm.WindowTitle = "Добавить нового клиента";

            addClientView.ShowDialog();

            if (addClientView.DialogResult == true)
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
                dbClients.Insert(0, client);
            }
        }
        public void OnEditClient()
        {
            if (SelectedClient != null)
            {
                var vm = new ClientViewModel(dbClients, dbOrders, dbOrdersItems, dbStockItems, dbExchangeRates, dbPayments, SelectedClient, clientRepo, orderRepo, exchangeRateRepo, orderItemRepo, stockItemRepo, paymentRepo);
                ClientView editClientView = new ClientView();

                editClientView.DataContext = vm;
                editClientView.Owner = App.Current.MainWindow;

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

                foreach (var order in dbOrders)
                {
                    if (SelectedClient.Id == order.Client.Id)
                    {
                        vm.Orders.Add(order);
                    }
                }

                editClientView.ShowDialog();

                if (editClientView.DialogResult == true)
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
                    dbClients.Remove(SelectedClient);
                }
            }
        }
    }
}
