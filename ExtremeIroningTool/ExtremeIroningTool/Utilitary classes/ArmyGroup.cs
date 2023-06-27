using ExtremeIroningTool.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class ArmyGroup : ILandForce, INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
        public General fieldmarshal = General.nullFieldMarshal;
        public General Commander
        {
            get { return fieldmarshal; }
            set
            {
                fieldmarshal = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Army> armies = new();
        public ObservableCollection<Army> Armies
        {
            get { return armies; }
            set { armies = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }


        private string pathToIcon = PathsToImages.DefaultArmyGroupIcon;
        public string PathToIcon
        {
            get { return pathToIcon; }
            set
            {
                pathToIcon = value;
                OnPropertyChanged();
            }
        }


        public ArmyGroup(General fieldmarshal, ObservableCollection<Army> armies, string name, 
            string pathToIcon)
        {
            Name = name;
            Commander = fieldmarshal;
            Armies = armies;
            PathToIcon = pathToIcon;
        }
        public ArmyGroup()
        {
            Name = UtilitaryFunctions.GetDefaultName("Army Group");
        }

        public override string ToString()
        {
            string ret = $"Army group {Name}, Field marshal {(Commander == null ? "None" : Commander)}, Armies {{";

            foreach (Army army in armies)
            {
                ret += army.ToString() + " ";
            }
            ret += "}";

            return ret;
        }
        public ArmyGroup Clone()
        {
            ObservableCollection<Army> clonedArmies = new();
            foreach (Army army in Armies) clonedArmies.Add(army.Clone());

            return new ArmyGroup()
            {
                Name = Name,
                Commander = General.nullFieldMarshal,
                PathToIcon = PathToIcon,
                Armies = clonedArmies
            };
        }
    }
}
