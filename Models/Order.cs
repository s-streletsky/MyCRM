using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    enum Status
    {
        Ready = 1,
        Awaiting_dispatch,
        Fully_paid,
        NEW,
        Billed,
        Partially_paid,
        Shipped
    }
    class Order
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Client Client { get; set; }
        public Status Status { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }

        public Order()
        {
            Created = DateTime.Today;
        }
    }
}
