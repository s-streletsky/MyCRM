using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.WPF;
using Newtonsoft.Json;

namespace CRM.ViewModels
{
    class AddNewClientViewModel : ViewModelBase
    {
        public RelayCommand SaveClientCommand { get; }
        public RelayCommand AddNewOrderCommand { get; }

        int? id;
        DateTime created;
        string name;
        string nickname;
        string phone;
        string email;
        Country country;
        string city;
        string address;
        ShippingMethod shippingMethod;
        string postalCode;
        string notes;

        ObservableCollection<Order> orders = new ObservableCollection<Order>();

        Client selectedClient;
        Database database;
        ClientRepository clientRepo = new ClientRepository();

        public int? Id 
        { 
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public string Nickname
        { 
            get { return nickname; }
            set { nickname = value; OnPropertyChanged();
            }
        }
        public string Phone
        {
            get { return phone; }
            set {
                phone = value;
                OnPropertyChanged();
            } 
        }
        public string Email
        {
            get { return email; }
            set {
                email = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                OnPropertyChanged();
            }
        }
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }
        public ShippingMethod ShippingMethod
        {
            get { return shippingMethod; }
            set
            {
                shippingMethod = value;
                OnPropertyChanged();
            }
        }
        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                postalCode = value;
                OnPropertyChanged();
            }
        }
        public string Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set { selectedClient = value; OnPropertyChanged(); }
        }
        public Database Database
        {
            get { return database; }
            set { database = value; OnPropertyChanged(); }
        }

        public AddNewClientViewModel(Client selectedClient, Database database)
        {
            this.Database = database;
            this.SelectedClient = selectedClient;

            AddNewOrderCommand = new RelayCommand(OnAddNewOrderButtonClick);
            SaveClientCommand = new RelayCommand(OnSaveClientButtonClick);
        }

        void OnSaveClientButtonClick(object _)
        {
            if (SelectedClient == null)
            {
                SelectedClient = new Client();
                SelectedClient.Created = DateTime.Now;

                SetClientProperties();

                var client = clientRepo.Add(SelectedClient);
                Database.Clients.Add(client);
            }
            else
            {
                SetClientProperties();
                clientRepo.Update(SelectedClient);
            }
        }

        private void OnClientMouseDoubleClick(object _)
        {
            SetClientProperties();
        }

        private void OnAddNewOrderButtonClick(object _)
        {
            //var order = new Order();
            //selectedClient.Orders.Add(order);
        }

        private void SetClientProperties()
        {
            selectedClient.Name = Name;
            selectedClient.Nickname = Nickname;
            selectedClient.Phone = Phone;
            selectedClient.Email = Email;
            selectedClient.Country = Country;
            selectedClient.City = City;
            selectedClient.Address = Address;
            selectedClient.ShippingMethod = ShippingMethod;
            selectedClient.PostalCode = PostalCode;
            selectedClient.Notes = Notes;
        }
    }
}
