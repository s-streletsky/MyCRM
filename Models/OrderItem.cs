using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal class OrderItem
    {
        private Order order;

        public int Id { get; set; }
        public Order Order { get { return order; } }
        public StockItem StockItem { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public float Total { get; set; }
        public float Profit { get; set; }
        public float Expenses { get; set; }
        public float ExchangeRate { get; set; }

        public OrderItem() { }
        public OrderItem(int id, Order order, StockItem item, float quantity, float price, float discount, float total, float profit, float expenses, float rate)
        {
            this.Id = id;
            this.order = order;
            this.StockItem = item;
            this.Quantity = quantity;
            this.Price = price;
            this.Discount = discount;
            this.Total = total;
            this.Profit = profit;
            this.Expenses = expenses;
            this.ExchangeRate = rate;
        }
    }
}
