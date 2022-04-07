using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CRM.Models;
using CRM.WPF;
using Newtonsoft.Json;

namespace CRM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private Currency currency;
        private decimal rate;

        public RelayCommand AddNewExchangeRateCommand { get; }
        public RelayCommand AddNewClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand AddNewItemCommand { get; }
        public RelayCommand DeleteClientCommand { get; }
        public RelayCommand LoadClientsCommand { get; }
        public RelayCommand LoadOrdersCommand { get; }
        public Database Database { get; set; }
        public Client SelectedClient { get; set; }

        public Currency Currency { get; set; }
        public decimal Rate { get; set; }

        private ClientRepository clientRepo = new ClientRepository();
        private OrderRepository orderRepo = new OrderRepository();
        private ExchangeRateRepository exRateRepo = new ExchangeRateRepository();

        public MainViewModel()
        {
            AddNewClientCommand = new RelayCommand(OnAddNewClient_Click);
            EditClientCommand = new RelayCommand(OnClientDoubleClick);
            AddNewItemCommand = new RelayCommand(OnAddNewItem_Click);
            DeleteClientCommand = new RelayCommand(OnDeleteItem_Click);
            LoadClientsCommand = new RelayCommand(OnLoadClients_Click);
            LoadOrdersCommand = new RelayCommand(OnLoadOrders_Click);
            AddNewExchangeRateCommand = new RelayCommand(OnAddNewExchangeRate_Click);

            this.Database = new Database();
            clientRepo.GetAll(Database);

            orderRepo.GetAll(Database);

            exRateRepo.GetAll(Database);

            this.SelectedClient = null;
            //Database.Currencies.Add(new Currency("UAH"));
        }
        public void OnAddNewClient_Click(object _)
        {
            //var selectedClientClone = JsonConvert.SerializeObject(SelectedClient, Formatting.Indented);

            var vm = new AddNewClientViewModel(SelectedClient, Database);
            AddNewClientView addNewClientView = new AddNewClientView();

            // if (vm.Result = true) { // add client }
            //if (Database.Clients.Count == 0)
            //{
            //    vm.Id = 1;
            //}
            //else
            //{
            //    vm.Id = Database.Clients[Database.Clients.Count - 1].Id + 1;
            //}          

            addNewClientView.DataContext = vm;
            //addNewClientView.SaveClientButton.Visibility = System.Windows.Visibility.Hidden;
            addNewClientView.Show();
        }

        public void OnClientDoubleClick(object _)
        {
            var vm = new AddNewClientViewModel(SelectedClient, Database);
            AddNewClientView addNewClientView = new AddNewClientView();

            addNewClientView.DataContext = vm;
            //addNewClientView.AddNewClientButton.Visibility = System.Windows.Visibility.Hidden;

            if (SelectedClient != null)
            {
                //vm.ClientIndex = Database.Clients.IndexOf(client);

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

            //addNewClientView.DataContext = vm;
            //addNewClientView.AddNewClientButton.Visibility = System.Windows.Visibility.Hidden;
            //addNewClientView.Show();
        }

        public void OnAddNewItem_Click(object _)
        {
            var vm = new AddNewItemViewModel(this);
            AddNewItemView addNewItemView = new AddNewItemView();

            if (Database.Stock.Count == 0)
            {
                vm.Id = 1;
            }
            else
            {
                vm.Id = Database.Stock[Database.Stock.Count - 1].Id + 1;
            }

            addNewItemView.DataContext = vm;
            addNewItemView.Show();
        }

        public void OnDeleteItem_Click(object _)
        {
            clientRepo.Delete(SelectedClient);
            var i = Database.Clients.IndexOf(SelectedClient);
            Database.Clients.RemoveAt(i);
        }

        public void OnLoadClients_Click(object _)
        {
            clientRepo.GetAll(Database);
        }

        public void OnLoadOrders_Click(object _)
        {
            orderRepo.GetAll(Database);
        }

        public void OnAddNewExchangeRate_Click(object _)
        {
            var newExRate = exRateRepo.Add(new ExchangeRate(Currency, Rate));
            Database.ExchangeRates.Insert(0, newExRate);
        }
    }
}
