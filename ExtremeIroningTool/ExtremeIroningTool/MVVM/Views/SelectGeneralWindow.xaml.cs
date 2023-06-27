using ExtremeIroningTool.Interfaces;
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
    /// Логика взаимодействия для SelectGeneralWindow.xaml
    /// </summary>
    public partial class SelectGeneralWindow : Window
    {
        public ILandForce? landForce;

        public SelectGeneralWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var general = (General)(e.AddedItems[0]);
                ((ViewModelBottomPartArmyConfigurator)DataContext).SetGeneral(general);
                var listbox = (ListBox)sender;
                listbox.SelectedItem = null;
            }
        }
    }
}
