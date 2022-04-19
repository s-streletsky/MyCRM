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
        private string name;

        public int? Id { get; set; }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }

        public Manufacturer() { }
        public Manufacturer(int id, string name) { Id = id; Name = name; }
    }
}
