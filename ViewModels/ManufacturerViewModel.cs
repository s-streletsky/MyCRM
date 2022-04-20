using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;
using CRM.Views;
using CRM.WPF;

namespace CRM.ViewModels
{
    internal class ManufacturerViewModel : ViewModelBase
    {
        private Manufacturer selectedManufacturer;
        private bool isListBoxEnabled;
        private string isAddStackPanelVisible;
        private bool isAddStackPanelEnabled;
        private string isEditStackPanelVisible;
        private string name;

        public RelayCommand AddManufacturerCommand { get; }
        public RelayCommand EditManufacturerCommand { get; }
        public RelayCommand DeleteManufacturerCommand { get; }
        public RelayCommand AddOKCommand { get; }
        public RelayCommand AddCancelCommand { get; }
        public RelayCommand EditOKCommand { get; }
        public RelayCommand EditCancelCommand { get; }
        public Manufacturer SelectedManufacturer { get { return selectedManufacturer; } set { selectedManufacturer = value; OnPropertyChanged(); } }
        public Database Database { get; set; }
        public ManufacturerRepository ManufacturerRepo { get; set; }
        public bool IsListBoxEnabled { get { return isListBoxEnabled; } set { isListBoxEnabled = value; OnPropertyChanged(); } }
        public string IsAddStackPanelVisible { get { return isAddStackPanelVisible; } set { isAddStackPanelVisible = value; OnPropertyChanged(); } }
        public bool IsAddStackPanelEnabled { get { return isAddStackPanelEnabled; } set { isAddStackPanelEnabled = value; OnPropertyChanged(); } }
        public string IsEditStackPanelVisible { get { return isEditStackPanelVisible; } set { isEditStackPanelVisible = value; OnPropertyChanged(); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        private List<string> Visibility { get; set; } = new List<string>() { "Hidden", "Visible" };

        public ManufacturerViewModel(Database db, ManufacturerRepository mfr)
        {
            Database = db;
            ManufacturerRepo = mfr;

            IsListBoxEnabled = true;
            IsAddStackPanelEnabled = false;
            IsAddStackPanelVisible = Visibility[1];
            IsEditStackPanelVisible = Visibility[0];

            AddManufacturerCommand = new RelayCommand(OnAddMenuButtonClick);
            EditManufacturerCommand = new RelayCommand(OnEditMenuButtonClick);
            DeleteManufacturerCommand = new RelayCommand(OnDeleteMenuButtonClick);
            AddOKCommand = new RelayCommand(OnAddOKButtonClick);
            AddCancelCommand = new RelayCommand(OnAddCancelButtonClick);
            EditOKCommand = new RelayCommand(OnEditOKButtonClick);
            EditCancelCommand = new RelayCommand(OnEditCancelButtonClick);           
        }

        // Кнопки "Добавить/Изменить/Удалить"
        public void OnAddMenuButtonClick(object _)
        {
            DisableListBox();
            Name = "Новый производитель";
        }

        public void OnEditMenuButtonClick(object _)
        {
            if (SelectedManufacturer != null)
            {
                IsAddStackPanelVisible = Visibility[0];
                IsEditStackPanelVisible = Visibility[1];

                DisableListBox();
                Name = SelectedManufacturer.Name;
            }
            else
            {
                Name = "Выберите запись из списка ниже!";
            }           
        }

        public void OnDeleteMenuButtonClick(object _)
        {
            ManufacturerRepo.Delete(SelectedManufacturer);
            var i = Database.Manufacturers.IndexOf(SelectedManufacturer);
            Database.Manufacturers.RemoveAt(i);
        }

        // Кнопки добавления новой записи
        public void OnAddOKButtonClick(object _)
        {
            var newManufacturer = ManufacturerRepo.Add(new Manufacturer(-1, Name));
            Database.Manufacturers.Add(newManufacturer);
            EnableListBox();
        }

        public void OnAddCancelButtonClick(object _)
        {
            EnableListBox();
        }

        // Кнопки редактирования записи
        public void OnEditOKButtonClick(object _)
        {
            SelectedManufacturer.Name = Name;
            ManufacturerRepo.Update(SelectedManufacturer);

            HideEditStackPanel();
            EnableListBox();
        }

        public void OnEditCancelButtonClick(object _)
        {
            HideEditStackPanel();
            EnableListBox();
        }

        // Вспомогательные методы
        private void EnableListBox()
        {
            IsListBoxEnabled = true;
            IsAddStackPanelEnabled = false;
            Name = "";
        }

        private void DisableListBox()
        {
            IsListBoxEnabled = false;
            IsAddStackPanelEnabled = true;            
        }

        private void HideEditStackPanel()
        {
            IsAddStackPanelVisible = Visibility[1];
            IsEditStackPanelVisible = Visibility[0];
        }
    }
}
