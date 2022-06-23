using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    internal class NewClientViewModel : ViewModelBase
    {
        private string name;
        public string Name { 
            get { return name; } 
            set { name = value; 
                OnPropertyChanged(); } 
        }
        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }

        public NewClientViewModel()
        {
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void CloseWindow(ICloseable window)
        {
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}
