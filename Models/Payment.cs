using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Client Client { get; set; }
        public Order Order { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }

        public Payment() { }
        public Payment(int id, DateTime date, Client client, Order order, float amount, string notes)
        {
            Id = id;
            Date = date;
            Client = client;
            Order = order;
            Amount = amount;
            Notes = notes;
        }

        public override string ToString()
        {
            var date = Date.ToShortDateString();
            return $"{date}, {Amount}, {Client.Name} ({Notes})";
        }
    }
}
