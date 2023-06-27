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
    /// Логика взаимодействия для SelectDivisionWindow.xaml
    /// </summary>
    public partial class SelectDivisionWindow : Window
    {
        public SelectDivisionWindow()
        {
            InitializeComponent();
        }

        private void SelectDivision_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            if (sender is ListBox listBox)
            {
                if (listBox.SelectedItem == null) return;
                ((ViewModelArmyConfigurator)DataContext).AddDivisionToCurrentArmy(
                    (Division)listBox.SelectedItem);
                ((ListBox)sender).SelectedItem = null;
            }
            else if (sender is ListBoxItem item)
            {
                ((ViewModelArmyConfigurator)DataContext).AddDivisionToCurrentArmy(item.DataContext as Division);
            }
            else if (sender is Button button)
            {
                ((ViewModelArmyConfigurator)DataContext).AddDivisionToCurrentArmy(button.Tag as Division);
            }
        }

        private void EditDivisionClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            var division = ((sender as Button).Tag as ListBoxItem).DataContext as Division;

            ((ViewModelArmyConfigurator)DataContext).EditDivisionClick(division);
        }
        private void DeleteDivisionClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            var division = ((sender as Button).Tag as ListBoxItem).DataContext as Division;

            ((ViewModelArmyConfigurator)DataContext).DeleteDivisionClick(division);
        }

        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectDivision_Click(sender, new());
        }
    }
}
