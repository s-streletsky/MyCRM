using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    internal class ExchangeRatesViewModel : ViewModelBase
    {
        private Currency selectedCurrency;
        private BindableFloat exchangeRate;
        private ExchangeRate selectedExchangeRate;

        public RelayCommand AddExchangeRateCommand { get; }
        public RelayCommand DeleteExchangeRateCommand { get; }
        public Database Database { get; set; }
        public ExchangeRateRepository ExchangeRateRepo { get; set; }
        public Currency SelectedCurrency { 
            get { return selectedCurrency; } 
            set { selectedCurrency = value; 
                OnPropertyChanged(); } 
        }
        public BindableFloat ExchangeRate { 
            get { return exchangeRate; } 
            set { exchangeRate = value; 
                OnPropertyChanged(); } 
        }
        public ExchangeRate SelectedExchangeRate { 
            get { return selectedExchangeRate; } 
            set { selectedExchangeRate = value; 
                OnPropertyChanged(nameof(IsDeleteButtonEnabled));
            } 
        }
        public bool IsDeleteButtonEnabled { 
            get { return SelectedExchangeRate != null; }
        }

        public ExchangeRatesViewModel() { }
        public ExchangeRatesViewModel(Database db, ExchangeRateRepository err)
        {
            Database = db;
            ExchangeRateRepo = err;

            ExchangeRate = new BindableFloat();

            SelectedCurrency = Currency.EUR;

            AddExchangeRateCommand = new RelayCommand(OnAddExchangeRate);
            DeleteExchangeRateCommand = new RelayCommand(OnDeleteExchangeRate);
        }

        private void OnAddExchangeRate()
        {
            var newExRate = ExchangeRateRepo.Add(new ExchangeRate(SelectedCurrency, ExchangeRate.Value));
            Database.ExchangeRates.Insert(0, newExRate);
        }
        private void OnDeleteExchangeRate()
        {
            ExchangeRateRepo.Delete(SelectedExchangeRate);
            Database.ExchangeRates.Remove(SelectedExchangeRate);
        }
    }
}
