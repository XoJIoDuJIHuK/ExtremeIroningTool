using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelGeneralSelectionCard
    {
        public GeneralSelectionCard view;
        public General general;

        public ViewModelGeneralSelectionCard(GeneralSelectionCard view, General general)
        {
            this.general = general;
            this.view = view;

            this.view.DataContext = this;
        }

        public string pathToIcon
        {
            get
            {
                return general == null ? "" : general.PathToIcon;
            }
        }
        public string name
        {
            get
            {
                return general == null ? "No general" : general.Name;
            }
        }
    }
}
