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
        public RelayCommand AddNewClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand AddNewItemCommand { get; }
        public RelayCommand SaveDatabaseCommand { get; }
        public RelayCommand LoadDatabaseCommand { get; }
        public Database Database { get; set; }
        public Client SelectedClient { get; set; }

        private Repository repo = new Repository();

        public MainViewModel()
        {
            AddNewClientCommand = new RelayCommand(OnAddNewClient_Click);
            EditClientCommand = new RelayCommand(OnClientDoubleClick);
            AddNewItemCommand = new RelayCommand(OnAddNewItem_Click);
            SaveDatabaseCommand = new RelayCommand(OnSaveDatabase_Click);
            LoadDatabaseCommand = new RelayCommand(OnLoadDatabase_Click);
            this.Database = new Database();
            this.SelectedClient = null;
            Database.Currencies.Add(new Currency("UAH"));
        }
        public void OnAddNewClient_Click(object _)
        {
            //var selectedClientClone = JsonConvert.SerializeObject(SelectedClient, Formatting.Indented);

            var vm = new AddNewClientViewModel(this);
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
            addNewClientView.SaveClientButton.Visibility = System.Windows.Visibility.Hidden;
            addNewClientView.Show();
        }

        public void OnClientDoubleClick(object _)
        {
            var selectedClientClone = JsonConvert.SerializeObject(SelectedClient, Formatting.Indented);

            var vm = new AddNewClientViewModel(selectedClientClone, this);
            AddNewClientView addNewClientView = new AddNewClientView();

            addNewClientView.DataContext = vm;
            addNewClientView.AddNewClientButton.Visibility = System.Windows.Visibility.Hidden;

            if (SelectedClient != null)
            {
                //vm.ClientIndex = Database.Clients.IndexOf(client);

                vm.Name = SelectedClient.Name;
                vm.Nickname = SelectedClient.Nickname;
                vm.Phone = SelectedClient.Phone;
                vm.Email = SelectedClient.Email;
                vm.Country = SelectedClient.Country;
                vm.City = SelectedClient.City;
                vm.Address = SelectedClient.Address;
                vm.ShippingMethod = SelectedClient.ShippingMethod;
                vm.Index = SelectedClient.Index;

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

        public void OnSaveDatabase_Click(object _)
        {
           repo.Save(Database);
        }

        public void OnLoadDatabase_Click(object _)
        {
            var db = repo.Load();

            foreach (var client in db.Clients)
            {
                Database.Clients.Add(client);
            }

            foreach (var item in db.Stock)
            {
                Database.Stock.Add(item);
            }
        }
    }
}
