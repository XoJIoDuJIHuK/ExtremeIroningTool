using ExtremeIroningTool.Interfaces;
using ExtremeIroningTool.MVVM.ViewModels;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.MVVM.Models
{
    public class ModelBottomPartArmyConfigurator
    {
        public AddArmyGroup addWindow;
        
        public SelectGeneralWindow selectGeneralWindow;
        public ModelBottomPartArmyConfigurator()
        {
            addWindow = new();
        }
    }
}
