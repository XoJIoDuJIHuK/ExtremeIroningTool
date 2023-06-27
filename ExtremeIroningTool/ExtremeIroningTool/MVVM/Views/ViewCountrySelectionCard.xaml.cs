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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ViewCountrySelectionCard.xaml
    /// </summary>
    public partial class ViewCountrySelectionCard : UserControl
    {
        public ViewModelCountrySelectionCard viewModel;
        public ViewCountrySelect parent;

        public ViewCountrySelectionCard()
        {
            InitializeComponent();
        }
    }
}
