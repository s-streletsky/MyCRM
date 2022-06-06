using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CRM.ViewModels
{
    internal class PaymentsViewModel : ViewModelBase
    {
        private readonly ICollectionView payments;
        private string search;

        public RelayCommand SearchCommand { get; }
        public ICollectionView Payments { get { return payments; } }
        public string Search { 
            get { return search; } 
            set { search = value; 
                OnPropertyChanged(); } 
        }

        public PaymentsViewModel(ObservableCollection<Payment> p)
        {
            payments = CollectionViewSource.GetDefaultView(p);
            SearchCommand = new RelayCommand(OnSearch);
        }

        private void OnSearch()
        {
            Payments.Filter = new Predicate<object>(p => Filter(p as Payment));
            Payments.Refresh();
        }
        private bool Filter(Payment payment)
        {
            return Search == null || payment.Client.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
