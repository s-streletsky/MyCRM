using CRM.Models;
using CRM.Views;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CRM.ViewModels
{
    internal class PaymentsViewModel : ViewModelBase
    {
        //private readonly ICollectionView payments;
        private ObservableCollection<Payment> payments;
        private string search;
        private PaymentRepository paymentRepo;
        private Payment selectedPayment;

        //public RelayCommand SearchCommand { get; }
        //public RelayCommand DeletePaymentCommand { get; }
        //public ICollectionView Payments { get { return payments; } }
        public RelayCommand EditPaymentCommand { get; }
        public RelayCommand DeletePaymentCommand { get; }
        public ObservableCollection<Payment> Payments { get { return payments; } set { payments = value; } }
        public Payment SelectedPayment { 
            get { return selectedPayment; } 
            set { selectedPayment = value; 
                OnPropertyChanged(nameof(IsEditDeleteButtonsEnabled)); } 
        }
        public string Search { 
            get { return search; } 
            set { search = value; 
                OnPropertyChanged(); } 
        }
        public bool IsEditDeleteButtonsEnabled { 
            get { return SelectedPayment != null; } 
        }

        public PaymentsViewModel(ObservableCollection<Payment> p, PaymentRepository pr)
        {
            payments = p;
            paymentRepo = pr;
            //var collectionViewSource = new CollectionViewSource();
            //collectionViewSource.Source = p;
            //payments = collectionViewSource.View;
            //SearchCommand = new RelayCommand(OnSearch);
            //DeletePaymentCommand = new RelayCommand(OnDeletePayment);
            EditPaymentCommand = new RelayCommand(OnEditPayment);
            DeletePaymentCommand = new RelayCommand(OnDeletePayment);
        }

        //private void OnSearch()
        //{
        //    Payments.Filter = new Predicate<object>(p => Filter(p as Payment));
        //    Payments.Refresh();
        //}
        //private bool Filter(Payment payment)
        //{
        //    return Search == null || payment.Client.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        //}

        //private void OnDeletePayment() { }
        public void OnEditPayment()
        {
            var selectedPaymentAmount = Convert.ToString(SelectedPayment.Amount);

            var vm = new OrderPaymentViewModel(SelectedPayment.Client, SelectedPayment.Order, selectedPaymentAmount);
            PaymentView paymentView = new PaymentView();
            paymentView.DataContext = vm;
            paymentView.ShowDialog();

            if (paymentView.DialogResult == true)
            {
                float amount;
                Single.TryParse(vm.Amount, out amount);

                SelectedPayment.Amount = amount;
                paymentRepo.Update(SelectedPayment);
            }
        }
        public void OnDeletePayment()
        {
            var userChoice = MessageBox.Show("Платёж от " + $"{SelectedPayment.Client.Name} на сумму {SelectedPayment.Amount}.\nУдалить?", "Удаление платежа", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (userChoice == MessageBoxResult.Yes)
            {
                paymentRepo.Delete(SelectedPayment);
                Payments.Remove(SelectedPayment);
            }
        }
    }
}
