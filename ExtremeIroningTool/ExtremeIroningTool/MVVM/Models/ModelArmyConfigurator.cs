using ExtremeIroningTool.MVVM.ViewModels;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtremeIroningTool.MVVM.Models
{
    public class ModelArmyConfigurator
    {
        public ViewModelArmyConfigurator viewModel;

        public Thickness selectedMargin = new(0);
        public Thickness unselectedMargin = new(10, 6, 10, 6);

        public SelectImageWindow selectImageWindow;
        public SelectDivisionWindow selectDivisionWindow;
        public DivisionContructorWindow divisionContructorWindow;

        public Division ConstructorDivision = new();

        public ModelArmyConfigurator(ViewModelArmyConfigurator viewModel)
        {
            this.viewModel = viewModel;

            selectImageWindow = new();
            selectImageWindow.Hide();
            selectImageWindow.DataContext = viewModel;

            selectDivisionWindow = new();
            selectDivisionWindow.Hide();
            selectDivisionWindow.DataContext = viewModel;
        }
    }
}
