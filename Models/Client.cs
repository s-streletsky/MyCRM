using CRM.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    enum Country
    {
        [Description("Украина")]
        Ukraine,
        [Description("США")]
        USA
    }

    enum ShippingMethod
    {
        [Description("Новая почта")]
        NovaPoshta = 1,
        [Description("Укрпочта")]
        Ukrposhta
    }
    
    class Client : ViewModelBase
    {
        private int? id;
        private DateTime? date;
        private string name;
        private string nickname;
        private string phone;
        private string email;
        private Country country;
        private string city;
        private string address;
        private ShippingMethod shippingMethod;
        private string postalCode;
        private string notes;

        public int? Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public DateTime? Date {
            get { return date; }
            set { date = value;
                OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged();
            }
        }
        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }
        public ShippingMethod ShippingMethod
        {
            get { return shippingMethod; }
            set
            {
                shippingMethod = value;
                OnPropertyChanged();
            }
        }
        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                postalCode = value;
                OnPropertyChanged();
            }
        }
        public string Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }

        public Client()
        {
            this.Id = -1;
            this.Date = DateTime.Now;
        }
        public Client(int? id, DateTime date, string name, string nickname, string phone, string email,
            Country country, string city, string address, ShippingMethod shippingMethod, 
            string postalCode, string notes)
        {
            this.Date = date;
            this.Id = id;
            this.Name = name;
            this.Nickname = nickname;
            this.Phone = phone;
            this.Email = email;
            this.Country = country;
            this.City = city;
            this.Address = address;
            this.ShippingMethod = shippingMethod;
            this.PostalCode = postalCode;
            this.Notes = notes;
        }
    }
}
