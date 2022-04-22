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
        public float Value { get; set; }

        public ExchangeRate() { }
        public ExchangeRate(Currency currency, float value)
        {
            Date = DateTime.Now;
            Currency = currency;
            Value = value;
        }
        public ExchangeRate(DateTime date, Currency currency, float value)
        {
            Date = date;
            Currency = currency;
            Value = value;
        }
    }
}
