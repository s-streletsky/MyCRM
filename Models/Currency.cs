using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class Currency
    {
        public string Code { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();

        public Currency(string code)
        {
            Code = code;
        }
    }
}
