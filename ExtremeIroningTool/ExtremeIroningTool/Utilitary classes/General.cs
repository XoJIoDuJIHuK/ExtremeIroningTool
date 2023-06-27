using ExtremeIroningTool.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExtremeIroningTool.Utilitary_classes
{
    public enum CommanderRank
    {
        General,
        Fieldmarshall
    }
    public class General : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public static readonly General nullGeneral = new("No general", 1, 1, PathsToImages.NullGeneral, CommanderRank.General, "SOV");
        public static readonly General nullFieldMarshal = new("No fieldmarshal", 1, 1, PathsToImages.NullGeneral, CommanderRank.Fieldmarshall, "SOV");

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private float attackBonus;
        public float AttackBonus
        {
            get { return attackBonus; }
            set
            {
                attackBonus = value;
                OnPropertyChanged();
            }
        }

        private float defenseBonus;
        public float DefenseBonus
        {
            get { return defenseBonus; }
            set
            {
                defenseBonus = value;
                OnPropertyChanged();
            }
        }

        private string pathToIcon;
        public string PathToIcon
        {
            get { return pathToIcon; }
            set
            {
                pathToIcon = value;
                OnPropertyChanged();
            }
        }

        private CommanderRank rank;
        public CommanderRank Rank
        {
            get
            {
                return rank;
            }
            set
            {
                rank = value;
                OnPropertyChanged();
            }
        }
        public string StringRank {
            get
            {
                return rank.ToString();
            }
        }

        private Country country;
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }

        public ILandForce? pinnedLandForce = null;

        public General(string name, float attackBonus, float defenceBonus, string pathToIcon, CommanderRank rank, string countryTag)
        {
            Name = name;
            AttackBonus = attackBonus;
            DefenseBonus = defenceBonus;
            PathToIcon = pathToIcon;
            Rank = rank;
            try
            {
                foreach (var c in DataBaseInteraction.allCountries)
                {
                    if (c.tag == countryTag)
                    {
                        Country = c;
                        break;
                    }
                }
                if (Country == null)
                {
                    throw new Exception("No such country tag");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        General()
        {
            Name = string.Empty;
            AttackBonus = 1;
            DefenseBonus = 1;
            PathToIcon = string.Empty;
            Rank = CommanderRank.General;
            Country = DataBaseInteraction.allCountries[0];
        }
        public override string ToString()
        {
            return $"{Name} AB:{AttackBonus} DB:{DefenseBonus}";
        }
    }
}
