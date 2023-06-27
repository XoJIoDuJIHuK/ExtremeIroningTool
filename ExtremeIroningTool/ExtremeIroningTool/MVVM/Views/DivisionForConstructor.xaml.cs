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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для DivisionForConstructor.xaml
    /// </summary>
    public partial class DivisionForConstructor : UserControl
    {
        public DivisionForConstructor()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModelBottomPartArmyConfigurator)(((DivisionForConstructor)((Grid)((Button)sender).Parent).Parent).Tag)).IncrementDivisionCount((Division)((UnitDictionaryElement)((Button)sender).DataContext).Key);
        }

        private void Reduce_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModelBottomPartArmyConfigurator)(((DivisionForConstructor)((Grid)((Button)sender).Parent).Parent).Tag)).DecrementDivisionCount((Division)((UnitDictionaryElement)((Button)sender).DataContext).Key);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModelBottomPartArmyConfigurator)(((DivisionForConstructor)((Grid)((Button)sender).Parent).Parent).Tag)).DeleteDivision((Division)((UnitDictionaryElement)((Button)sender).DataContext).Key);
        }
    }
}
