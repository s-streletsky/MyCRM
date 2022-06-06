using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.ViewModels
{
    internal class StatsViewModel : ViewModelBase
    {
        private Database database;
        
        private string selectedMonth;
        private int selectedYear;

        private int ordersTotal;
        private int ordersNew;
        private int ordersinProgress;
        private int ordersClosed;
        private float paymentsTotal;
        private float income;
        private float expenses;
        private float profit;

        private List<string> months;
        private List<int> years;

        public RelayCommand PrintStatsCommand { get; }

        public int OrdersTotal { 
            get { return ordersTotal; } 
            set { ordersTotal = value; 
                OnPropertyChanged(); } 
        }
        public int OrdersNew { 
            get { return ordersNew; } 
            set { ordersNew = value; 
                OnPropertyChanged(); } 
        }
        public int OrdersInProgress { 
            get { return ordersinProgress; } 
            set { ordersinProgress = value;
                OnPropertyChanged(); } 
        }
        public int OrdersClosed { 
            get { return ordersClosed; } 
            set { ordersClosed = value; 
                OnPropertyChanged(); } 
        }
        public float PaymentsTotal { 
            get { return paymentsTotal; } 
            set { paymentsTotal = value; 
                OnPropertyChanged(); } 
        }
        public float Income { 
            get { return income; } 
            set { income = value;
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
        public List<string> Months { get { return months; } }
        public List<int> Years { get { return years; } }
        public string SelectedMonth { 
            get { return selectedMonth; } 
            set { selectedMonth = value; 
                OnPropertyChanged(); } 
        }
        public int SelectedYear { 
            get { return selectedYear; } 
            set { selectedYear = value; 
                OnPropertyChanged(); } 
        }

        public StatsViewModel(Database db)
        {
            months = new List<string>(13) { "Весь год", "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            years = new List<int>(10);

            database = db;

            PrintStatsCommand = new RelayCommand(PrintStats);

            var firstOrder = database.Orders.Last<Order>();
            DateTime firstOrderDate = firstOrder.Date;
            var firstOrderYear = firstOrderDate.Year;

            var current = DateTime.Today;

            while (firstOrderYear <= current.Year)
            {
                years.Insert(0, firstOrderYear);
                firstOrderYear++;
            }

            SelectedMonth = Months[current.Month];
            SelectedYear = current.Year;
        }    

        internal void PrintStats()
        {
            var month = Months.IndexOf(SelectedMonth);
            DateTime periodStart;
            DateTime periodEnd;

            if (month != 0)
            {
                periodStart = new DateTime(SelectedYear, month, 1);
                periodEnd = new DateTime(SelectedYear, month, DateTime.DaysInMonth(SelectedYear, month), 23, 59, 59);
            }
            else
            {
                periodStart = new DateTime(SelectedYear, 1, 1);
                periodEnd = new DateTime(SelectedYear, 12, 31, 23, 59, 59);
            }            

            OrdersTotal = OrdersNew = OrdersInProgress = OrdersClosed = 0;

            foreach (var order in database.Orders)
            {
                if (order.Date >= periodStart && order.Date <= periodEnd)
                {
                    OrdersTotal++;

                    switch (order.Status)
                    {
                        case OrderStatus.NEW:
                            OrdersNew++;
                            break;
                        case OrderStatus.Shipped:
                            OrdersClosed++;
                            break;                       
                        default:
                            OrdersInProgress++;
                            break;
                    }
                }
            }

            PaymentsTotal = 0;

            foreach (var payment in database.Payments)
            {
                if (payment.Date >= periodStart && payment.Date <= periodEnd)
                {
                    PaymentsTotal += payment.Amount;
                }
            }

            Income = Expenses = Profit = 0;

            foreach (var order in database.Orders)
            {
                if (order.Date >= periodStart && order.Date <= periodEnd)
                {
                    Income += order.Total;
                    Expenses += order.Expenses;
                    Profit += order.Profit;
                }                
            }
        }
    }
}
