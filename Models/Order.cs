using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;

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
    class Order : ViewModelBase
    {
        private int id;
        private DateTime date;
        private Client client;
        private OrderStatus status;
        private float total;
        private float expenses;
        private float profit;
        private string notes;

        public int Id { 
            get { return id; }
            set { id = value; }
        }
        public DateTime Date { 
            get { return date; }
        }
        public Client Client { 
            get { return client; } 
            set { client = value; 
                OnPropertyChanged(); } 
        }
        public OrderStatus Status { 
            get { return status; } 
            set { status = value; 
                OnPropertyChanged(); } 
        }
        public float Total { 
            get { return total; } 
            set { total = value; 
                OnPropertyChanged(); } 
        }
        public float Expenses { 
            get { return expenses; } 
            set { expenses = value; 
                OnPropertyChanged(); } 
        }
        public float Profit { 
            get { return profit; } 
            set { profit = value; 
                OnPropertyChanged(); } 
        }
        public string Notes { 
            get { return notes; } 
            set { notes = value; 
                OnPropertyChanged(); } 
        }

        public Order()
        {
            date = DateTime.Now;
        }

        public Order(DateTime dt, Client c, OrderStatus s)
        {
            this.date = dt;
            this.Client = c;
            this.Status = s;
            this.Total = 0;
            this.Notes = "";
        }

        public Order(int id, DateTime dt, Client c, OrderStatus s, float total, float exp, float profit, string notes)
        {
            this.id = id;
            this.date = dt;
            this.Client = c;
            this.Status = s;
            this.Total = total;
            this.Expenses = exp;
            this.Profit = profit;
            this.Notes = notes;
        }
    }
}
