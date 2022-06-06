using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;

namespace CRM.Models
{
    internal class OrderItem : ViewModelBase
    {
        private int id;
        private Order order;
        private StockItem stockItem;
        private float quantity;
        private float price;
        private float discount;
        private float total;
        private float profit;
        private float expenses;
        public float exchangeRate;

        public int Id { 
            get { return id; } 
            set { id = value; } 
        }
        public Order Order { get { return order; } }
        public StockItem StockItem { get { return stockItem; } }
        public float Quantity { 
            get { return quantity; } 
            set { quantity = value; 
                OnPropertyChanged(); } 
        }
        public float Price { get { return price; } }
        public float Discount { 
            get { return discount; } 
            set { discount = value; 
                OnPropertyChanged(); } 
        }
        public float Total { 
            get { return total; } 
            set { total = value; 
                OnPropertyChanged(); } 
        }
        public float Profit { 
            get { return profit; } 
            set { profit = value; 
                OnPropertyChanged(); } 
        }
        public float Expenses { 
            get { return expenses; } 
            set { expenses = value; 
                OnPropertyChanged(); } 
        }
        public float ExchangeRate { get { return exchangeRate; } }

        public OrderItem() { }
        public OrderItem(int id, Order order, StockItem item, float quantity, float price, float discount, float total, float profit, float expenses, float rate)
        {
            this.id = id;
            this.order = order;
            this.stockItem = item;
            this.Quantity = quantity;
            this.price = price;
            this.Discount = discount;
            this.Total = total;
            this.Profit = profit;
            this.Expenses = expenses;
            this.exchangeRate = rate;
        }
    }
}
