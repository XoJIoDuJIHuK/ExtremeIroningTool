using ExtremeIroningTool.MVVM.ViewModels;
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
    public class Division : Unit, INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        private ObservableCollection<UnitDictionaryElement> battalions = new();
        public ObservableCollection<UnitDictionaryElement>  Battalions
        {
            get { return battalions; }
            set
            {
                battalions = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SupportCompany> supportCompanies = new();
        public ObservableCollection<SupportCompany> SupportCompanies
        {
            get { return supportCompanies; }
            set
            {
                supportCompanies = value;
                OnPropertyChanged();
            }
        }
        
        public string PathToIcon
        {
            get { return pathToIcon == string.Empty || pathToIcon == " " ? PathsToImages.DefaultDivisionIcon : pathToIcon; }
            set
            {
                pathToIcon = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return name == string.Empty ? "EmptyName" : name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public Division(string name, string pathToIcon, ObservableCollection<UnitDictionaryElement> batallions,
            ObservableCollection<SupportCompany> supportCompanies) : base(name, pathToIcon)
        {
            Battalions = batallions;
            SupportCompanies = supportCompanies;

            SummarizeStats();
        }
        public Division() : base() { }

        public void SummarizeStats()
        {
            int divisionSize = supportCompanies.Count;
            foreach (var b in battalions)
            {
                divisionSize += b.Value;
            }

            health = organization = softAttack = hardAttack = defence = breakthrough = armor = piercing = 0;
            frontWidth = 0;
            foreach(var tm in modifiers)
            {
                tm.attackModifier = 0;
                tm.defenseModifier = 0;
            }

            double averageArmor = 0;
            double maxArmor = 0;
            double averagePiercing = 0;
            double maxPiercing = 0;

            foreach (var b in battalions)
            {
                averageArmor += b.Key.armor * b.Value / divisionSize;
                if (b.Key.armor > maxArmor) maxArmor = b.Key.armor;

                averagePiercing += b.Key.piercing * b.Value / divisionSize;
                if (b.Key.piercing > maxPiercing) maxPiercing = b.Key.piercing;

                health += b.Key.health * b.Value;
                organization += b.Key.organization * b.Value / divisionSize;
                softAttack += b.Key.softAttack * b.Value;
                hardAttack += b.Key.hardAttack * b.Value;
                defence += b.Key.defence * b.Value;
                breakthrough += b.Key.breakthrough * b.Value;
                frontWidth += b.Key.frontWidth * b.Value;
                for (int i = 0; i < modifiers.Count; i++)
                {
                    modifiers[i].attackModifier += b.Key.modifiers[i].attackModifier * b.Value / UnitsCount;
                    modifiers[i].defenseModifier += b.Key.modifiers[i].defenseModifier * b.Value / UnitsCount;
                }
            }
            foreach (var s in supportCompanies)
            {
                averageArmor += s.armor / divisionSize;
                if (s.armor > maxArmor) maxArmor = s.armor;

                averagePiercing += s.piercing / divisionSize;
                if (s.piercing > maxPiercing) maxPiercing = s.piercing;

                health += s.health;
                organization += s.organization / divisionSize;
                softAttack += s.softAttack;
                hardAttack += s.hardAttack;
                defence += s.defence;
                breakthrough += s.breakthrough;

                for (int i = 0; i < modifiers.Count; i++)
                {
                    modifiers[i].attackModifier += s.modifiers[i].attackModifier / UnitsCount;
                    modifiers[i].defenseModifier += s.modifiers[i].defenseModifier / UnitsCount;
                }
            }

            armor = 0.4 * maxArmor + 0.6 * averageArmor;
            armor = 0.4 * maxPiercing + 0.6 * averagePiercing;
        }//Stats calculations

        public override string ToString()
        {
            string ret = $"Division {name}, Battalions {{ ";

            foreach (var b in battalions)
            {
                ret += $"{b.Key}-{b.Value} ";
            }
            foreach (var s in supportCompanies)
            {
                ret += $"{s} ";
            }
            ret += "}";

            return ret;
        }
        public Division Clone()
        {
            var ret = new Division()
            {
                name = name.Clone().ToString(),
                pathToIcon = pathToIcon.Clone().ToString()
            };

            foreach (var b in Battalions) ret.Battalions.Add(new(b.Key, b.Value));
            foreach (var s in SupportCompanies) ret.SupportCompanies.Add(s);

            ret.SummarizeStats();

            return ret;
        }

        public Division SurfaceClone()
        {
            var ret = new Division(Name, PathToIcon, Battalions, SupportCompanies)
            {
                modifiers = modifiers,
            };
            ret.SummarizeStats();

            return ret;
        }

        public double MaxHealth;
        public double MaxOrganisation;

        public int UnitsCount
        {
            get
            {
                int ret = 0;
                foreach (var b in Battalions) ret += b.Value;
                return ret + SupportCompanies.Count;
            }
        }

        public double HealthPart
        {
            get
            {
                return health / MaxHealth;
            }
        }
        public double OrganizationPart
        {
            get
            {
                return organization / MaxOrganisation;
            }
        }

        public void UpdateProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(PathToIcon));
            OnPropertyChanged(nameof(Battalions));
            OnPropertyChanged(nameof(SupportCompanies));
        }

        public void UpdateBattleStats()
        {
            OnPropertyChanged(nameof(HealthPart));
            OnPropertyChanged(nameof(OrganizationPart));
        }

        public void Heal()
        {
            health = MaxHealth;
            organization = MaxOrganisation;
        }

        public double GetBattalionTypePart(string type)
        {
            int amountOfBattalions = 0;
            int amountOfRightBattalions = 0;

            foreach (var b in Battalions)
            {
                amountOfBattalions += b.Value;
                if (DataBaseInteraction.IsBattalionOfType(b.Key.name, type)) amountOfRightBattalions += b.Value;
            }

            return amountOfBattalions == 0 ? 0 : amountOfRightBattalions / amountOfBattalions;
        }
    }
}
