using ExtremeIroningTool.MVVM.Models;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public enum StatusBarMessageType
    {
        None,
        Success,
        Error
    }

    public class ViewModelArmyConfigurator : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private List<BottomPartArmyConfigurator> countries;
        public int selectedCountryIndex = 0;

        public ObservableCollection<ArmyGroup> ArmyGrops
        {
            get { return DataBaseInteraction.DBArmyGroups; }
        }
        public ObservableCollection<Division> Divisions
        {
            get { return DataBaseInteraction.DBDivisions; }
        }
        public ObservableCollection<ArmyGroup> ListBoxArmyGroups
        {
            get
            {
                return DataBaseInteraction.DBArmyGroups;
            }
        }
        public ICommand BackArmyConfiguratorClickCommand
        {
            get
            {
                return view.mainWindow.viewModel.BackArmyConfiguratorClickCommand;
            }
        }
        public ICommand ForwardArmyConfiguratorClickCommand
        {
            get
            {
                return view.mainWindow.viewModel.ForwardArmyConfiguratorClickCommand;
            }
        }

        public ICommand switchToFirstCountry { get; set; }
        public ICommand switchToSecondCountry { get; set; }
        public ICommand hideAddWindow { get; set; }
        public ICommand hideEditIconWindow { get; set; }
        public ICommand hideSelectDivisionWindow { get; set; }

        public ICommand AddEmptyArmyGroupCommand { get; set; }

        #region Edit Unit Icons

        private UnitType typeOfIconsToEdit = UnitType.Division;
        public UnitType TypeOfIconsToEdit
        {
            get { return typeOfIconsToEdit; }
            set
            {
                typeOfIconsToEdit = value;
                OnPropertyChanged(nameof(CurrentListOfIcons));
            }
        }

        public List<BitmapImage> CurrentListOfIcons
        {
            get {
                return typeOfIconsToEdit == UnitType.Division ? DataBaseInteraction.DBDivisionIcons : typeOfIconsToEdit == 
                    UnitType.Army ? DataBaseInteraction.DBArmyIcons : DataBaseInteraction.DBArmyGroupIcons;
            }
        }

        public void ChangeUnitIcon(BitmapImage image)
        {
            if (image == null) return;
            string path = image.UriSource.ToString();
            switch (TypeOfIconsToEdit)
            {
                case UnitType.Division:
                    {
                        model.divisionContructorWindow.viewModel.ConstructorDivision.PathToIcon = path;
                        model.divisionContructorWindow.viewModel.UpdateDivisionProperties();
                        break;
                    }
                case UnitType.Army:
                    {
                        countries[selectedCountryIndex].viewModel.SetArmyIcon(path);
                        break;
                    }
                case UnitType.ArmyGroup:
                    {
                        countries[selectedCountryIndex].viewModel.SetArmyGroupIcon(path);
                        break;
                    }
            }
            model.selectImageWindow.Hide();
        }

        #endregion

        #region Country Icons
        private int FirstCountryIndex { get { return view.mainWindow.viewModel.model.selectedCountries[0]; } }
        public string FirstCountryIcon { get { return DataBaseInteraction.allCountries[FirstCountryIndex].pathToIcon; } }
        private int SecondCountryIndex { get { return view.mainWindow.viewModel.model.selectedCountries[1]; } }
        public string SecondCountryIcon { get { return DataBaseInteraction.allCountries[SecondCountryIndex].pathToIcon; } }
        #endregion

        public ArmyConfigurator view;
        public ModelArmyConfigurator model;
            
        public void ShowStatusBarMessage(StatusBarMessageType type, string message)
        {
            countries[selectedCountryIndex].viewModel.SetStatusBar(type, message);
        }

        public AddArmyGroup addWindow { get; set; }
        public ViewModelArmyConfigurator(ArmyConfigurator view)
        {
            this.view = view;
            this.view.DataContext = this;
            model = new(this);

            countries = new();

            int i = 0;
            foreach (var b in view.CenterColumn.Children)
            {
                if (b is BottomPartArmyConfigurator bottomPart)
                {
                    countries.Add(bottomPart);
                    bottomPart.parentView = view;
                    bottomPart.index = i++;
                }
            }//set selected countries

            switchToFirstCountry = new RelayCommand(SwitchToFirstCountry);
            switchToSecondCountry = new RelayCommand(SwitchToSecondCountry);
            hideAddWindow = new RelayCommand(HideAddWindow);
            hideEditIconWindow = new RelayCommand(HideEditIconWindow);

            CreateNewDivision = new RelayCommand(AddNewDivisionClick);
            HideSelectDivisionWindow = new RelayCommand(HideSelectDivisionWindowClick);

            AddEmptyArmyGroupCommand = new RelayCommand(AddEmptyArmyGroupToCurrentCountry);

            addWindow = new()
            {
                WindowState = WindowState.Normal,
                WindowStyle = WindowStyle.None,
                DataContext = this
            };//may cause troubles

        }
        public void InitializeVMS()
        {
            int i = 0;
            foreach (BottomPartArmyConfigurator b in countries)
            {
                b.viewModel = new(b, ref i == 0 ? ref view.mainWindow.viewModel.model.Attackers : ref view.mainWindow.viewModel.model.Defenders);
                i++;
            }

            model.divisionContructorWindow = new(view);
            model.divisionContructorWindow.Hide();
        }

        public void SwitchToFirstCountry()
        {
            view.firstCountry.Visibility = Visibility.Visible;
            view.secondCountry.Visibility = Visibility.Collapsed;
            view.FirstFlag.Margin = model.selectedMargin;
            view.SecondFlag.Margin = model.unselectedMargin;

            selectedCountryIndex = 0;
        }
        public void SwitchToSecondCountry()
        {
            view.firstCountry.Visibility = Visibility.Collapsed;
            view.secondCountry.Visibility = Visibility.Visible;
            view.FirstFlag.Margin = model.unselectedMargin;
            view.SecondFlag.Margin = model.selectedMargin;

            selectedCountryIndex = 1;
        }

        public void HideAddWindow()
        {
            addWindow.Hide();
        }

        public void HideEditIconWindow()
        {
            model.selectImageWindow.Hide();
        }
        public void ShowEditIconWindow(UnitType unitType)
        {
            model.selectImageWindow.Owner = unitType == UnitType.Division ? model.divisionContructorWindow :
                view.mainWindow;
            TypeOfIconsToEdit = unitType;
            model.selectImageWindow.ShowDialog();
        }

        public void CountryIconsChanged()
        {
            OnPropertyChanged(nameof(FirstCountryIcon));
            OnPropertyChanged(nameof(SecondCountryIcon));
        }

        public void AddEmptyArmyGroupToCurrentCountry()
        {
            countries[selectedCountryIndex].viewModel.AddGroup(new ArmyGroup());
        }


        #region Select Division Window
        private ViewModelDivisionConstructorWindow vmdcw
        {
            get { return model.divisionContructorWindow.viewModel; }
        }

        public ICommand CreateNewDivision { get; set; }
        public void AddDivisionToCurrentArmy(Division division)
        {
            countries[selectedCountryIndex].viewModel.AddDivisionToCurrentArmy(division);
        }

        public void AddNewDivisionClick()
        {
            vmdcw.EditMode = false;
            model.divisionContructorWindow.Show();
            vmdcw.InitConstructorDivision();
        }
        public void EditDivisionClick(Division division)
        {
            vmdcw.EditMode = true;
            vmdcw.ConstructorDivision = division.Clone();
            vmdcw.OriginalDivision = division;
            model.divisionContructorWindow.ShowDialog();
            model.divisionContructorWindow.viewModel.InitConstructorDivision();
        }

        public void DeleteDivisionClick(Division division)
        {
            bool deleteResult = DataBaseInteraction.Delete(division);

            if (deleteResult) ShowStatusBarMessage(StatusBarMessageType.Success, $"Division {division.Name}");
            else ShowStatusBarMessage(StatusBarMessageType.Error, $"Could not delete division {division.Name}");

            OnPropertyChanged(nameof(Divisions));
        }

        public ICommand HideSelectDivisionWindow { get; set; }
        public void HideSelectDivisionWindowClick()
        {
            model.selectDivisionWindow.Hide();
        }

        public void UpdateDivisionList()
        {
            OnPropertyChanged(nameof(Divisions));
        }

        #endregion

        #region General Selection

        public Country CurrentCountry {  get { return AllCountries[SelectedCountryIndex]; } }
        private List<Country> AllCountries { get { return DataBaseInteraction.allCountries; } }
        private int SelectedCountryIndex { get { return view.mainWindow.viewModel.model.selectedCountries[selectedCountryIndex]; } }

        #endregion

        public void Hide()
        {
            view.Visibility = Visibility.Collapsed;
        }
        public void Show()
        {
            view.Visibility = Visibility.Visible;
        }
    }
}
