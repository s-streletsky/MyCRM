using CRM.Models;
using CRM.WPF;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CRM.ViewModels
{
    internal class ChooseStockItemViewModel : ViewModelBase
    {
        private string search;
        private StockItem selectedItem;
        private readonly ICollectionView stockItems;

        public RelayCommand<ICloseable> CloseWindowCommand { get; private set; }
        public RelayCommand SearchCommand { get; }

        public string Search {
            get { return search; }
            set { search = value;
                OnPropertyChanged(); }
        }
        public ICollectionView StockItems {
            get { return stockItems; }
        }
        public StockItem SelectedItem {
            get { return selectedItem; }
            set { selectedItem = value;
                OnPropertyChanged(); }
        }

        public ChooseStockItemViewModel() { }
        public ChooseStockItemViewModel(ObservableCollection<StockItem> i)
        {
            stockItems = CollectionViewSource.GetDefaultView(i);
            SearchCommand = new RelayCommand(OnSearch);
            CloseWindowCommand = new RelayCommand<ICloseable>(CloseWindow);
        }

        private void OnSearch()
        {
            StockItems.Filter = new Predicate<object>(o => Filter(o as StockItem));
            StockItems.Refresh();
        }
        private bool Filter(StockItem item)
        {
            return Search == null || item.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
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
