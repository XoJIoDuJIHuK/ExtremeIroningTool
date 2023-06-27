using ExtremeIroningTool.MVVM.ViewModels;
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
    /// Логика взаимодействия для SelectImageWindow.xaml
    /// </summary>
    public partial class SelectImageWindow : Window
    {
        public bool IsArmyConfiguratorWindow = true;
        public SelectImageWindow()
        {
            InitializeComponent();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsArmyConfiguratorWindow)
            {
                ((ViewModelArmyConfigurator)DataContext).ChangeUnitIcon((BitmapImage)((ListBox)sender).
                SelectedItem);
            }
            else
            {
                ((ViewModelModifiers)DataContext).ChangeIcon((BitmapImage)((ListBox)sender).
                SelectedItem);
            }
        }
    }
}
