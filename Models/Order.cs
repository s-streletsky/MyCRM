using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    enum OrderStatus
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
        public int? Id { get; set; }
        public DateTime? Created { get; set; }
        public Client Client { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }

        public Order()
        {
            Created = DateTime.Today;
        }

        public Order(int? id, DateTime? created, Client client, OrderStatus status, decimal total, string notes)
        {
            this.Id = id;
            this.Created = created;
            this.Client = client;
            this.Status = status;
            this.Total = total;
            this.Notes = notes;

        }
    }
}
