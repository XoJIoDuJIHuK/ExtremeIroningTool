using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelCountrySelectionCard
    {
        public ICommand countryClick { get; set; }

        public ViewCountrySelectionCard view;
        public Country country;
        public int index;
        public string CountryName
        {
            get
            {
                return country.name;
            }
        }
        public string PathToIcon
        {
            get
            {
                return country.pathToIcon;
            }
        }
        public ViewModelCountrySelectionCard(ViewCountrySelectionCard view, Country country, int index)
        {
            this.country = country;
            this.view = view;
            view.DataContext = this;
            this.index = index;

            countryClick = new RelayCommand(CountryClick);
        }

        public void CountryClick()
        {
            view.parent.viewModel.CountryClick(index);
        }
        
    }
}
