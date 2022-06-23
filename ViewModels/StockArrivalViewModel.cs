using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.WPF;
using CRM.Models;
using Microsoft.Toolkit.Mvvm.Input;

namespace CRM.ViewModels
{
    internal class StockArrivalViewModel : ViewModelBase
    {
        private StockItem stockItem;
        private BindableFloat quantity;
        private bool isAddGridEnabled;
        private string isAddGridVisible;
        private string isEditGridVisible;
        private bool isDataGridEnabled;
        private StockArrival selectedArrival;

        public RelayCommand AddStockArrivalCommand { get; }
        public RelayCommand EditStockArrivalCommand { get; }
        public RelayCommand DeleteStockArrivalCommand { get; }
        public RelayCommand AddOKCommand { get; }
        public RelayCommand AddCancelCommand { get; }
        public RelayCommand EditOKCommand { get; }
        public RelayCommand EditCancelCommand { get; }
        public Database Database { get; set; }
        public StockArrivalRepository StockArrivalRepo { get; set;}
        public StockItemRepository StockItemRepo { get; set; }
        public StockItem StockItem { 
            get { return stockItem; } 
            set { stockItem = value; 
                OnPropertyChanged(); } 
        }
        public BindableFloat Quantity { 
            get { return quantity; } 
            set { quantity = value; 
                OnPropertyChanged(); } 
        }
        public StockArrival SelectedArrival { 
            get { return selectedArrival; } 
            set { selectedArrival = value; 
                OnPropertyChanged(nameof(IsEditDeleteButtonsEnabled)); } 
        }
        public bool IsAddGridEnabled { 
            get { return isAddGridEnabled; } 
            set { isAddGridEnabled = value; 
                OnPropertyChanged(); } 
        }
        public string IsAddGridVisible { 
            get { return isAddGridVisible; } 
            set { isAddGridVisible = value; 
                OnPropertyChanged(); } 
        }
        public string IsEditGridVisible { 
            get { return isEditGridVisible; } 
            set { isEditGridVisible = value; 
                OnPropertyChanged(); } 
        }
        public bool IsDataGridEnabled { 
            get { return isDataGridEnabled; } 
            set { isDataGridEnabled = value; 
                OnPropertyChanged(); } 
        }
        public bool IsEditDeleteButtonsEnabled { 
            get { return SelectedArrival != null; } 
        }

        public StockArrivalViewModel(Database db, StockArrivalRepository sar, StockItemRepository sir)
        {
            Database = db;
            StockArrivalRepo = sar;
            StockItemRepo = sir;

            IsAddGridEnabled = false;
            IsAddGridVisible = "Visible";
            IsEditGridVisible = "Hidden";
            IsDataGridEnabled = true;

            Quantity = new BindableFloat();
            Quantity.Input = "0";

            AddStockArrivalCommand = new RelayCommand(OnAddStockArrival);
            EditStockArrivalCommand = new RelayCommand(OnEditStockArrival);
            DeleteStockArrivalCommand = new RelayCommand(OnDeleteStockArrival);

            AddOKCommand = new RelayCommand(OnAddOK);
            AddCancelCommand = new RelayCommand(OnAddCancel);
            EditOKCommand = new RelayCommand(OnEditOK);
            EditCancelCommand = new RelayCommand(OnEditCancel);
        }

        // Кнопки "Добавить/Изменить/Удалить"
        private void OnAddStockArrival()
        {
            DisableDataGrid();
        }

        private void OnEditStockArrival()
        {
            Quantity.Input = SelectedArrival.Quantity.ToString();
            StockItem = SelectedArrival.StockItem;

            DisableDataGrid();
            IsAddGridVisible = "Hidden";
            IsEditGridVisible = "Visible";
        }

        private void OnDeleteStockArrival()
        {
            StockArrivalRepo.Delete(SelectedArrival);
            StockItemRepo.UpdateQuantity(SelectedArrival.StockItem);

            var i = Database.StockArrivals.IndexOf(SelectedArrival);
            Database.StockArrivals.RemoveAt(i);           
        }

        // Кнопки добавления новой записи
        private void OnAddOK()
        {
            var newArrival = StockArrivalRepo.Add(new StockArrival(StockItem, Quantity.Value));
            Database.StockArrivals.Insert(0, newArrival);
            StockItemRepo.UpdateQuantity(newArrival.StockItem);

            ClearEnteredData();
            EnableDataGrid();                        
        }

        private void OnAddCancel()
        {
            ClearEnteredData();
            EnableDataGrid();
        }

        // Кнопки редактирования записи
        private void OnEditOK()
        {
            SelectedArrival.Quantity = Quantity.Value;
            StockArrivalRepo.Update(SelectedArrival);
            StockItemRepo.UpdateQuantity(SelectedArrival.StockItem);

            ClearEnteredData();
            HideEditGrid();
            EnableDataGrid();
        }

        private void OnEditCancel()
        {
            ClearEnteredData();
            HideEditGrid();
            EnableDataGrid();
        }

        // Вспомогательные методы
        private void EnableDataGrid()
        {
            IsDataGridEnabled = true;
            IsAddGridEnabled = false;
        }

        private void DisableDataGrid()
        {
            IsDataGridEnabled = false;
            IsAddGridEnabled = true;
        }

        private void HideEditGrid()
        {
            IsAddGridVisible = "Visible";
            IsEditGridVisible = "Hidden";
        }

        private void ClearEnteredData()
        {
            StockItem = null;
            Quantity.Input = "0";
            Quantity.Value = 0;
        }
    }
}
