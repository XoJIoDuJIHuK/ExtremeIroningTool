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
    /// Логика взаимодействия для AddArmyGroup.xaml
    /// </summary>
    public partial class AddArmyGroup : Window
    {
        public ViewModelAddArmyGroup viewModel;
        public BottomPartArmyConfigurator calledCountry;
        public AddArmyGroup()
        {
            InitializeComponent();
            viewModel = new(this);
        }

        private void armyGroupCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;

            if (listbox.SelectedItem != null)
            {
                var armyGroup = (ArmyGroup)listbox.SelectedItem;

                viewModel.AddArmyGroup(armyGroup);
            }

            listbox.SelectedItem = null;
        }
    }
}
