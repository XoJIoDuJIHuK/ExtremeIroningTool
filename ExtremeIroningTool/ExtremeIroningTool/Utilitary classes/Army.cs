using ExtremeIroningTool.Interfaces;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
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
    public class Army : ILandForce, INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        private ObservableCollection<UnitDictionaryElement> divisions = new();
        public ObservableCollection<UnitDictionaryElement> Divisions
        {
            get { return divisions; }
            set
            {
                divisions = value;
                OnPropertyChanged();
            }
        }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private General general = General.nullGeneral;
        public General Commander
        {
            get { return general; }
            set
            {
                general = value;
                OnPropertyChanged();
            }
        }

        private string pathToIcon = PathsToImages.DefaultArmyIcon;
        public string PathToIcon
        {
            get { return pathToIcon; }
            set
            {
                pathToIcon = value;
                OnPropertyChanged();
            }
        }

        public Army(ObservableCollection<UnitDictionaryElement> divisions, string name, General general, string pathToIcon)
        {
            Divisions = divisions;
            Name = name;
            Commander = general;
            PathToIcon = pathToIcon;
        }
        public Army()
        {
            Name = UtilitaryFunctions.GetDefaultName("Army");
            Commander = General.nullGeneral;
            pathToIcon = PathsToImages.DefaultArmyIcon;
        }
        public override string ToString()
        {
            string ret = $"Army {name}, General {(general == null ? "None" : general)}, Divisions {{ ";
            foreach (var d in Divisions)
            {
                ret += d.Key.ToString() + "-" + d.Value.ToString() + " ";
            }
            ret += "}";

            return ret;
        }
        public Army Clone()
        {
            var clonedDivisions = new ObservableCollection<UnitDictionaryElement>();
            foreach (var d in Divisions) clonedDivisions.Add(new(d.Key, d.Value));

            return new Army()
            {
                Name = Name,
                Commander = General.nullGeneral,
                PathToIcon = PathToIcon,
                Divisions = clonedDivisions
            };
        }

        public ObservableCollection<Division> Armies
        {
            get
            {
                var ret = new ObservableCollection<Division>();

                foreach (var d in Divisions)
                {
                    ret.Add((Division)d.Key);
                }
                return ret;
            }
        }

        public ObservableCollection<Division> battleDivisions = new();
        public ObservableCollection<Division> BattleDivisions
        {
            get
            {
                return battleDivisions;
            }
            set
            {
                battleDivisions = value;
                OnPropertyChanged();
            }
        }

        public void SetUpBattleArmies()
        {
            BattleDivisions.Clear();

            foreach (var d in Divisions)
            {
                for (int i = 0; i < d.Value; i++)
                {
                    BattleDivisions.Add(((Division)d.Key).SurfaceClone());
                }
            }

            OnPropertyChanged(nameof(BattleDivisions));
        }

        public void UpdateProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(PathToIcon));
            OnPropertyChanged(nameof(Divisions));
            OnPropertyChanged(nameof(Armies));
            OnPropertyChanged(nameof(BattleDivisions));
        }
    }
}
