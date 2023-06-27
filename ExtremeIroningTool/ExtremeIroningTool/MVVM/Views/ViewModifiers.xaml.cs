using ExtremeIroningTool.MVVM.ViewModels;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ViewModifiers.xaml
    /// </summary>
    public partial class ViewModifiers : UserControl
    {
        public MainWindow mainWindow;
        public ViewModelModifiers viewModel;

        public ViewModifiers()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Delete(((sender as Button).DataContext as ViewModelModifiers.ProxyModifier).self);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Edit(((sender as Button).DataContext as ViewModelModifiers.ProxyModifier).self);
        }

        private void FirstCountry_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.CheckFirstCheckbox((sender as CheckBox).DataContext as ViewModelModifiers.ProxyModifier);
        }

        private void SecondCountry_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.CheckSecondCheckbox((sender as CheckBox).DataContext as ViewModelModifiers.ProxyModifier);
        }
    }
}
