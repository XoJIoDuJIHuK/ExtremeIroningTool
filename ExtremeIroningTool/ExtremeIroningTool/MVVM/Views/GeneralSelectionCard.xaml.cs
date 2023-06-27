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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для GeneralSelectionCard.xaml
    /// </summary>
    /// 
    public partial class GeneralSelectionCard : UserControl
    {
        public BottomPartArmyConfigurator parentView
        {
            get { return (BottomPartArmyConfigurator)Tag; }
            set { parentView.Tag = value; }
        }
        public GeneralSelectionCard()
        {
            InitializeComponent();
        }
        public void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button Sender = (Button)sender;
            var ag = (ILandForce)((ListBoxItem)Sender.Tag).Content;

            switch (Sender.Name)
            {
                case "Icon":
                    {
                        parentView.viewModel.GeneralClick(ag);
                        break;
                    }
                case "DeleteButton":
                    {
                        parentView.viewModel.DeleteLandForce(ag);
                        break;
                    }
                case "SelectButton":
                    {
                        parentView.viewModel.ShowArmies(ag);
                        break;
                    }
                default: 
                    {
                        break;
                    }
            }
        }
    }
}
