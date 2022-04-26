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
        private float exchangeRate;

        public RelayCommand AddExchangeRateCommand { get; }
        public RelayCommand DeleteExchangeRateCommand { get; }
        public Database Database { get; set; }
        public ExchangeRateRepository ExchangeRateRepo { get; set; }
        public Currency SelectedCurrency { get { return selectedCurrency; } set { selectedCurrency = value; OnPropertyChanged(); } }
        public float ExchangeRate { get { return exchangeRate; } set { exchangeRate = value; OnPropertyChanged(); } }
        public ExchangeRate SelectedExchangeRate { get; set; }

        public ExchangeRatesViewModel() { }
        public ExchangeRatesViewModel(Database db, ExchangeRateRepository err)
        {
            Database = db;
            ExchangeRateRepo = err;

            SelectedCurrency = Currency.EUR;

            AddExchangeRateCommand = new RelayCommand(OnAddExchangeRateClick);
            DeleteExchangeRateCommand = new RelayCommand(OnDeleteExchangeRateClick);
        }

        // Кнопка "Добавить"
        private void OnAddExchangeRateClick()
        {
            var newExRate = ExchangeRateRepo.Add(new ExchangeRate(SelectedCurrency, ExchangeRate));
            Database.ExchangeRates.Insert(0, newExRate);
        }

        // Кнопка "Удалить"
        private void OnDeleteExchangeRateClick()
        {
            ExchangeRateRepo.Delete(SelectedExchangeRate);
            var i = Database.ExchangeRates.IndexOf(SelectedExchangeRate);
            Database.ExchangeRates.RemoveAt(i);
        }
    }
}
