using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var view = new DbSelectView();
            //view.showdialog();
            //if (view.DialogResult != true) { return; }

            var vm = new MainViewModel();
            var view = new MainView();
            view.DataContext = vm;
            view.ShowDialog();
        }
    }
}
