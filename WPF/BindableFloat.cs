using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.WPF
{
    internal class BindableFloat : ViewModelBase
    {
        private float value;
        private string input;

        public float Value { 
            get { return value; } 
            set { this.value = value; 
                OnPropertyChanged(); } 
        }
        public string Input { 
            get { return input; } 
            set { this.input = value;
                this.value = Convert.ToSingle(value);
                OnPropertyChanged(); } 
        }
    }
}
