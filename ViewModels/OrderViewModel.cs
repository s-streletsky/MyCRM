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
        private Client client;
        private DateTime date;
        private OrderStatus orderStatus;
        private ObservableCollection<OrderItem> orderItems = new ObservableCollection<OrderItem>();

        public RelayCommand ChooseClientCommand { get; }
        public Client Client { 
            get { return client; } 
            set { client = value;
                OnPropertyChanged(); 
            } 
        }
        public DateTime Date { get { return date; } }
        public OrderStatus OrderStatus { 
            get { return orderStatus; } 
            set { orderStatus = value; 
                OnPropertyChanged(); } }
        public Database Database { get; set; }

        public OrderViewModel() { }
        public OrderViewModel(Database db)
        {
            date = DateTime.Now;
            OrderStatus = OrderStatus.NEW;
            Database = db;

            ChooseClientCommand = new RelayCommand(OnChooseClient);
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
    }
}
