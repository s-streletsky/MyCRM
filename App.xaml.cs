using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace CRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            //var currentCulture = CultureInfo.CurrentUICulture;

            //Thread.CurrentThread.CurrentCulture = currentCulture;
            //Thread.CurrentThread.CurrentUICulture = currentCulture;
            //CultureInfo.DefaultThreadCurrentCulture = currentCulture;
            //CultureInfo.DefaultThreadCurrentUICulture = currentCulture;

            base.OnStartup(e);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));

            ////var view = new DbSelectView();
            ////view.showdialog();
            ////if (view.DialogResult != true) { return; }

            //var vm = new MainViewModel();
            //var view = new MainView();
            //view.DataContext = vm;
            //view.ShowDialog();
        }
    }
}
