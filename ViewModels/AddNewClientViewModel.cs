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
        public RelayCommand AddClientCommand { get; }
        public RelayCommand EditClientCommand { get; }
        public RelayCommand AddNewOrderCommand { get; }

        private Client selectedClientClone;
        private Repository repo = new Repository();

        public Client SelectedClientClone
        { 
            get { return selectedClientClone; } 
            set { selectedClientClone = value; OnPropertyChanged(); } }

        string name;
        string nickname;
        string phone;
        string email;
        ObservableCollection<Order> orders;
        Country country;
        string city;
        string address;
        ShippingMethod shippingMethod;
        string index;

        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set { 
                name = value;
                OnPropertyChanged();

            }
        }
        public string Nickname
        { 
            get { return nickname; }
            set {
                nickname = value;
                OnPropertyChanged();
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
        public string Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel mainViewModel;

        public AddNewClientViewModel()
        {
            SelectedClientClone = new Client(1);
            AddClientCommand = new RelayCommand(OnAddClientButton_Click);
        }

        public AddNewClientViewModel(MainViewModel mainViewModel)
        {
            AddClientCommand = new RelayCommand(OnAddClientButton_Click);
            EditClientCommand = new RelayCommand(OnClientMouseDoubleClick);
            AddNewOrderCommand = new RelayCommand(OnAddNewOrderButton_Click);
            this.mainViewModel = mainViewModel;
        }

        public AddNewClientViewModel(string selectedClientClone, MainViewModel mainViewModel)
        {
            this.selectedClientClone = JsonConvert.DeserializeObject<Client>(selectedClientClone);
            AddClientCommand = new RelayCommand(OnAddClientButton_Click);
            EditClientCommand = new RelayCommand(OnClientMouseDoubleClick);
            AddNewOrderCommand = new RelayCommand(OnAddNewOrderButton_Click);
            this.mainViewModel = mainViewModel;
        }

        void OnAddClientButton_Click(object _)
        {
            // Close Window / Set Window Result (true)
            var client = new Client(Id, Name, Nickname, Phone, Email, Country, City, Address, ShippingMethod, Index);
            mainViewModel.Database.Clients.Add(client);

            Name = null;
            Nickname = null;
            Phone = null;
            Email = null;
            Id = Id + 1;
        }

        void OnClientMouseDoubleClick(object _)
        {           
            selectedClientClone.Name = Name;
            selectedClientClone.Nickname = Nickname;
            selectedClientClone.Phone = Phone;
            selectedClientClone.Email = Email;
            selectedClientClone.Country = Country;
            selectedClientClone.City = City;
            selectedClientClone.Address = Address;
            selectedClientClone.ShippingMethod = ShippingMethod;
            selectedClientClone.Index = Index;
        }

        void OnAddNewOrderButton_Click(object _)
        {
            var order = new Order();
            selectedClientClone.Orders.Add(order);
        }
    }
}
