using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<ExchangeRate> dbExchangeRates;

        private ExchangeRateRepository exchangeRateRepo;

        private Currency selectedCurrency;
        private BindableFloat exchangeRate;
        private ExchangeRate selectedExchangeRate;

        public RelayCommand AddExchangeRateCommand { get; }
        public RelayCommand DeleteExchangeRateCommand { get; }

        public ObservableCollection<ExchangeRate> DbExchangeRates { get { return dbExchangeRates; } }
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

        public ExchangeRatesViewModel(ObservableCollection<ExchangeRate> er, ExchangeRateRepository err)
        {
            dbExchangeRates = er;
            exchangeRateRepo = err;

            ExchangeRate = new BindableFloat();

            SelectedCurrency = Currency.EUR;

            AddExchangeRateCommand = new RelayCommand(OnAddExchangeRate);
            DeleteExchangeRateCommand = new RelayCommand(OnDeleteExchangeRate);
        }

        private void OnAddExchangeRate()
        {
            var newExRate = exchangeRateRepo.Add(new ExchangeRate(SelectedCurrency, ExchangeRate.Value));
            dbExchangeRates.Insert(0, newExRate);
        }
        private void OnDeleteExchangeRate()
        {
            exchangeRateRepo.Delete(SelectedExchangeRate);
            dbExchangeRates.Remove(SelectedExchangeRate);
        }
    }
}
