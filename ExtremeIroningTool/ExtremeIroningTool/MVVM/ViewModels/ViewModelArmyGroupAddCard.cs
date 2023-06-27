using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelArmyGroupAddCard
    {
        public ViewArmyGroupAddCard view;
        public ArmyGroup armyGroup;
        public string Name
        {
            get
            {
                if (armyGroup != null) return armyGroup.Name;
                else return "New army group";
            }
        }
        public string PathToIcon
        {
            get
            {
                if (armyGroup != null) return armyGroup.PathToIcon;
                else return "./../../Images/Button Icons/add empty army group.png";
            }
        }
        public ViewModelArmyGroupAddCard(ViewArmyGroupAddCard view, ArmyGroup armyGroup)
        {
            this.view = view;
            this.armyGroup = armyGroup;

            if (armyGroup == null) view.Armies.Visibility = Visibility.Collapsed;
        }
    }
}
