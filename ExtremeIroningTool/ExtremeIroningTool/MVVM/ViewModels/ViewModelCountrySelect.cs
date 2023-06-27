using ExtremeIroningTool.MVVM.Views;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.Intrinsics.X86;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExtremeIroningTool.Utilitary_classes;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelCountrySelect : DependencyObject, INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Dependency properties

        public ICommand BackCountrySelectClickCommand { get; set; }
        public ICommand ForwardCountrySelectClickCommand { get; set; }

        public DependencyProperty firstSignVisibilityProperty = DependencyProperty.Register(
            "firstSignVisibility",
            typeof(Visibility),
            typeof(ViewModelCountrySelect),
            new PropertyMetadata(null));

        public DependencyProperty secondSignVisibilityProperty = DependencyProperty.Register(
            "secondSignVisibility",
            typeof(Visibility),
            typeof(ViewModelCountrySelect),
            new PropertyMetadata(null));

        public DependencyProperty firstSignColumnProperty = DependencyProperty.Register(
            "firstSignColumn",
            typeof(int),
            typeof(ViewModelCountrySelect),
            new PropertyMetadata(null));

        public DependencyProperty secondSignColumnProperty = DependencyProperty.Register(
            "secondSignColumn",
            typeof(int),
            typeof(ViewModelCountrySelect),
            new PropertyMetadata(null));

        public Visibility firstSignVisibility
        {
            get { return (Visibility)GetValue(firstSignVisibilityProperty); }
            set { SetValue(firstSignVisibilityProperty, value); }
        }
        public Visibility secondSignVisibility
        {
            get { return (Visibility)GetValue(secondSignVisibilityProperty); }
            set { SetValue(secondSignVisibilityProperty, value); }
        }
        public int firstSignColumn
        {
            get { return (int)GetValue(firstSignColumnProperty); }
            set { SetValue(firstSignColumnProperty, value); }
        }
        public int secondSignColumn
        {
            get { return (int)GetValue(secondSignColumnProperty); }
            set { SetValue(secondSignColumnProperty, value); }
        }

        #endregion

        private int lastSelectedCountry;
        public int LastSelectedCountry
        {
            get { return lastSelectedCountry; }
            set
            {
                lastSelectedCountry = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CountryPosterPath));
                OnPropertyChanged(nameof(CountryDescription));
                OnPropertyChanged(nameof(CountryName));
            }
        }

        public string CountryPosterPath
        {
            get
            {
                return DataBaseInteraction.allCountries[LastSelectedCountry].pathToPoster;
            }
        }
        public string CountryDescription
        {
            get
            {
                return DataBaseInteraction.allCountries[LastSelectedCountry].description;
            }
        }
        public string CountryName
        {
            get
            {
                return DataBaseInteraction.allCountries[LastSelectedCountry].name;
            }
        }

        public ViewCountrySelect view;
        public ViewModelCountrySelect(ViewCountrySelect viewCountrySelect)
        {
            view = viewCountrySelect;
            view.DataContext = this;

            int i = 0;

            foreach (var c in view.CountriesIcons.Children)
            {
                if (c is ViewCountrySelectionCard b && i < 4)
                {
                    b.viewModel = new ViewModelCountrySelectionCard(b, 
                        DataBaseInteraction.allCountries[i], i);
                    b.DataContext = b.viewModel;
                    b.parent = view;
                    i++;
                }
            }

            BackCountrySelectClickCommand = new RelayCommand(view.mainWindow.viewModel.
                BackCountrySelectClick);
            ForwardCountrySelectClickCommand = new RelayCommand(view.mainWindow.viewModel.
                ForwardCountrySelectClick);

            SetValue(firstSignVisibilityProperty, Visibility.Visible);
            SetValue(secondSignVisibilityProperty, Visibility.Visible);

            SetValue(firstSignColumnProperty, 0);
            SetValue(secondSignColumnProperty, 2);
        }

        public void CountryClick(int index)
        {
            LastSelectedCountry = index;
            int countryIndex = view.mainWindow.viewModel.model.selectedCountries.IndexOf(index);
            int nullIndex = view.mainWindow.viewModel.model.selectedCountries.IndexOf(-1);
            if (countryIndex != -1)//if country is already selected, unselect it
            {
                view.mainWindow.viewModel.model.selectedCountries[countryIndex] = -1;
                if (countryIndex == 0) SetValue(firstSignVisibilityProperty, Visibility.Collapsed);
                else SetValue(secondSignVisibilityProperty, Visibility.Collapsed);
            }
            else if (nullIndex != -1)//if there is a free slot, select country
            {
                view.mainWindow.viewModel.model.selectedCountries[nullIndex] = index;
                if (nullIndex == 0)
                {
                    SetValue(firstSignVisibilityProperty, Visibility.Visible);
                    SetValue(firstSignColumnProperty, index * 2);
                }
                else
                {
                    SetValue(secondSignVisibilityProperty, Visibility.Visible);
                    SetValue(secondSignColumnProperty, index * 2);
                }
            }
            view.mainWindow.ArmyConfigurator.viewModel.CountryIconsChanged();
            view.mainWindow.Modifiers.viewModel.UpdateFlags(); 
        }

        public void Hide()
        {
            view.Visibility = Visibility.Collapsed;
            view.mainWindow.viewModel.ClearArmiesFromCommanders();
        }
        public void Show()
        {
            view.Visibility = Visibility.Visible;
        }
    }
}
