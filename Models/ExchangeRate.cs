using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class ExchangeRate
    {
        DateTime Date { get; set; }
        decimal Value { get; set; }

        public ExchangeRate(decimal value)
        {
            Date = DateTime.Now;
            Value = Math.Round(value, 2);
        }
    }
}
