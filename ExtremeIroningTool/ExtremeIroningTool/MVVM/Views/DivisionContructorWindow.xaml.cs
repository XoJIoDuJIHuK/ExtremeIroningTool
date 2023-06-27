using ExtremeIroningTool.MVVM.ViewModels;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для DivisionContructorWindow.xaml
    /// </summary>
    public partial class DivisionContructorWindow : Window
    {
        public ViewModelDivisionConstructorWindow viewModel;
        public ArmyConfigurator parentView;
        public DivisionContructorWindow(ArmyConfigurator parent)
        {
            parentView = parent;

            viewModel = new(this);
            DataContext = viewModel;

            InitializeComponent();
        }

        private void AddUnit_Click(object sender, RoutedEventArgs e)
        {
            //sender as Button .DataContext is Battalion
            if ((sender as Button).DataContext is Battalion)
            {
                viewModel.AddUnit((sender as Button).DataContext as Battalion);
            }
            else
            {
                viewModel.AddUnit((sender as Button).DataContext as SupportCompany);
            }
        }
        private void ChangeUnit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is UnitDictionaryElement)
            {
                viewModel.ChangeUnitClick(((sender as Button).DataContext as UnitDictionaryElement).Key as Battalion);
            }
            else
            {
                viewModel.ChangeUnitClick((sender as Button).DataContext as SupportCompany);
            }
        }

        private void IncrementBattalion_Click(object sender, RoutedEventArgs e)
        {
            var battalion = (Battalion)((UnitDictionaryElement)((Button)sender).DataContext).Key;
            viewModel.IncrementBattalion(battalion);
        }
        private void DecrementBattalion_Click(object sender, RoutedEventArgs e)
        {
            var battalion = (Battalion)((UnitDictionaryElement)((Button)sender).DataContext).Key;
            viewModel.DecrementBattalion(battalion);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton r)
            {
                if (r.Tag is string s) viewModel.ApplyFilter(s);
            }
        }
    }
}
