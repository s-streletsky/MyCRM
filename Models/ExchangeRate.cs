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

        public ExchangeRate(decimal rate)
        {
            Date = DateTime.Now;
            Rate = Math.Round(rate, 2);
        }
    }
}
