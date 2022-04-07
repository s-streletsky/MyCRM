using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    enum Currency
    {
        EUR = 1,
        USD,
        UAH
    }
    internal class ExchangeRate
    {
        public DateTime Date { get; set; }
        public Currency Currency { get; set; }
        public decimal Rate { get; set; }

        public ExchangeRate()
        {

        }

        public ExchangeRate(Currency currency, decimal rate)
        {
            Date = DateTime.Now;
            Currency = currency;
            Rate = Math.Round(rate, 2);
        }

        public ExchangeRate(DateTime date, Currency currency, decimal rate)
        {
            Date = date;
            Currency = currency;
            Rate = Math.Round(rate, 2);
        }
    }
}
