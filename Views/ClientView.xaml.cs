using CRM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CRM.WPF;
using CRM.Models;

namespace CRM
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class ClientView : Window, ICloseable
    {
        public ClientView()
        {
            InitializeComponent();
        }
    }
}
