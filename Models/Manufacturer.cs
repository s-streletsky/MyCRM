using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;

namespace CRM.Models
{
    internal class Manufacturer : ViewModelBase
    {
        private int? id;
        private string name;

        public int? Id { get { return id; } set { id = value; OnPropertyChanged(); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }

        public Manufacturer() { }
        public Manufacturer(int id, string name) { Id = id; Name = name; }
    }
}
