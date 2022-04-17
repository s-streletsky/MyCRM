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
        public RelayCommand AddOkCommand { get; }
        public RelayCommand AddCancelCommand { get; }
        public RelayCommand EditOkCommand { get; }
        public RelayCommand EditCancelCommand { get; }
        public Manufacturer SelectedManufacturer { get { return selectedManufacturer; } set { selectedManufacturer = value; OnPropertyChanged(); } }
        public Database Database { get; set; }
        public ManufacturerRepository MfRepo { get; set; }
        public bool IsListBoxEnabled { get { return isListBoxEnabled; } set { isListBoxEnabled = value; OnPropertyChanged(); } }
        public string IsAddStackPanelVisible { get { return isAddStackPanelVisible; } set { isAddStackPanelVisible = value; OnPropertyChanged(); } }
        public bool IsAddStackPanelEnabled { get { return isAddStackPanelEnabled; } set { isAddStackPanelEnabled = value; OnPropertyChanged(); } }
        public string IsEditStackPanelVisible { get { return isEditStackPanelVisible; } set { isEditStackPanelVisible = value; OnPropertyChanged(); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        private List<string> Visibility { get; set; } = new List<string>() { "Hidden", "Visible" };

        public ManufacturerViewModel(Database db, ManufacturerRepository mfRepo)
        { 
            MfRepo = mfRepo;
            IsListBoxEnabled = true;
            IsAddStackPanelEnabled = false;
            IsAddStackPanelVisible = Visibility[1];
            IsEditStackPanelVisible = Visibility[0];
            AddManufacturerCommand = new RelayCommand(OnAddMenuButtonClick);
            EditManufacturerCommand = new RelayCommand(OnEditMenuButtonClick);
            DeleteManufacturerCommand = new RelayCommand(OnDeleteMenuButtonClick);
            AddOkCommand = new RelayCommand(OnAddOkButtonClick);
            AddCancelCommand = new RelayCommand(OnAddCancelButtonClick);
            EditOkCommand = new RelayCommand(OnEditOkButtonClick);
            EditCancelCommand = new RelayCommand(OnEditCancelButtonClick);

            this.Database = db;
        }

        public void OnAddMenuButtonClick(object _)
        {
            DisableListBox();

            Name = "-- Новый производитель --";
        }

        public void OnEditMenuButtonClick(object _)
        {
            IsAddStackPanelVisible = Visibility[0];
            IsEditStackPanelVisible = Visibility[1];
            DisableListBox();
            Name = SelectedManufacturer.Name;
        }

        public void OnDeleteMenuButtonClick(object _)
        {
            
        }

        public void OnAddOkButtonClick(object _)
        {
            var newManufacturer = MfRepo.Add(new Manufacturer(-1, Name));
            Database.Manufacturers.Add(newManufacturer);

            Name = "";

            EnableListBox();
        }

        public void OnAddCancelButtonClick(object _)
        {
            EnableListBox();
        }

        public void OnEditOkButtonClick(object _)
        {

        }

        public void OnEditCancelButtonClick(object _)
        {

        }

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
    }
}
