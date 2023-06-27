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
    /// Логика взаимодействия для BattalionEditor.xaml
    /// </summary>
    public partial class BattalionEditor : UserControl
    {
        public MainWindow mainWindow;
        public ViewModelBattalionEditor viewModel;
        public BattalionEditor()
        {
            InitializeComponent();
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editedBattalion = e.Row.Item as Battalion;
            var editedText = ((TextBox)e.EditingElement).Text;
            var property = e.Column.Header;
            if (!viewModel.UpdateBattalion(editedBattalion.name, property as string, editedText))
            {
                switch (property as string)
                {
                    case "Health":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.health.ToString();
                            break;
                        }
                    case "Organization":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.organization.ToString();
                            break;
                        }
                    case "SoftAttack":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.softAttack.ToString();
                            break;
                        }
                    case "HardAttack":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.hardAttack.ToString();
                            break;
                        }
                    case "Defense":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.defence.ToString();
                            break;
                        }
                    case "Breakthrough":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.breakthrough.ToString();
                            break;
                        }
                    case "Armor":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.armor.ToString();
                            break;
                        }
                    case "Piercing":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.piercing.ToString();
                            break;
                        }
                    case "FrontWidth":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.frontWidth.ToString();
                            break;
                        }
                    case "PathToIcon":
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.pathToIcon.ToString();
                            break;
                        }
                    default:
                        {
                            ((TextBox)e.EditingElement).Text = editedBattalion.vehicleRatio.ToString();
                            break;
                        }
                }
            }
        }
    }
}
