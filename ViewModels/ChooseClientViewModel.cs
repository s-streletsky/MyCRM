using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    internal class ChooseClientViewModel : ViewModelBase
    {
        private string search;
        private Client selectedClient;
        private readonly ICollectionView clients;

        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
        public RelayCommand SearchCommand { get; }
        
        public string Search { 
            get { return search; } 
            set { search = value; 
                OnPropertyChanged(); } 
        }
        public ICollectionView Clients {
            get { return clients; } 
        }
        public Client SelectedClient { 
            get { return selectedClient; } 
            set { selectedClient = value; 
                OnPropertyChanged(); } 
        }

        public ChooseClientViewModel() { }
        public ChooseClientViewModel(ObservableCollection<Client> c)
        {
            var collectionViewSource = new CollectionViewSource();
            collectionViewSource.Source = c;
            clients = collectionViewSource.View;
            SearchCommand = new RelayCommand(OnSearch);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void OnSearch()
        {
            Clients.Filter = new Predicate<object>(o => Filter(o as Client));
            Clients.Refresh();
        }
        private bool Filter(Client client)
        {
            return Search == null || client.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
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
