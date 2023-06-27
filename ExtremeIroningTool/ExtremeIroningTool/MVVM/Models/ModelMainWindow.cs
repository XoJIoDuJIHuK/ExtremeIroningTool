using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.MVVM.Models
{
    public class ModelMainWindow
    {

        public List<int> selectedCountries = new() { 0, 1 };

        public LandForces Attackers = new();
        public LandForces Defenders = new();

        public ModelMainWindow(){}
    }
}
