using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class LandForces
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public static HashSet<string> allDivisionNames = new HashSet<string>();
        public static HashSet<string> allArmyNames = new HashSet<string>();
        public static HashSet<string> allArmyGroupNames = new HashSet<string>();

        public ObservableCollection<ArmyGroup> ArmyGroups = new();

        public LandForces()
        {

        }
        public ObservableCollection<Division> AllDivisions = new();

        public void UpdateDivision(Division newDivision, string oldName)
        {
            foreach (var group in ArmyGroups)
            {
                foreach (var army in group.Armies)
                {
                    foreach (var d in army.Divisions)
                    {
                        //if (d.Key == division) OnPropertyChanged(nameof(d.Key));
                        if (d.Key.name == oldName)
                        {
                            d.Key = newDivision;
                            d.UpdateDivision();
                            break;
                        }
                    }
                    army.UpdateProperties();
                }
            }
        }
        public void UpdateDivisionsBattleStats()
        {
            foreach (var d in AllDivisions) d.UpdateBattleStats();
        }
    }
}
