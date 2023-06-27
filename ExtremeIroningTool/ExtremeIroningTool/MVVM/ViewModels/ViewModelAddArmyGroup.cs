using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelAddArmyGroup
    {
        public AddArmyGroup view;
        public ViewModelAddArmyGroup(AddArmyGroup view)
        {
            this.view = view;
        }
        

        public void AddArmyGroup(ArmyGroup sender)
        {
            view.calledCountry.viewModel.AddGroup(sender);
        }
    }
}
