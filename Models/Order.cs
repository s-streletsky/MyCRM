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
        private int id;
        private DateTime? created;

        public int Id { 
            get { return id; }
            set { id = value; }
        }
        public DateTime? Created { 
            get { return created; }
        }
        public Client Client { get; set; }
        public OrderStatus Status { get; set; }
        public float Total { get; set; }
        public string Notes { get; set; }

        public Order()
        {
            created = DateTime.Now;
        }

        public Order(DateTime? created, Client client, OrderStatus status)
        {
            this.created = created;
            this.Client = client;
            this.Status = status;
        }

        public Order(int id, DateTime? created, Client client, OrderStatus status, float total, string notes)
        {
            this.id = id;
            this.created = created;
            this.Client = client;
            this.Status = status;
            this.Total = total;
            this.Notes = notes;

        }
    }
}
