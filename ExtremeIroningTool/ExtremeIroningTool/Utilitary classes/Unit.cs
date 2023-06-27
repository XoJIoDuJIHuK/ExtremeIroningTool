using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.Utilitary_classes
{
    public enum UnitType
    {
        Division,
        Army,
        ArmyGroup
    }
    public abstract class Unit : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public BitmapImage Image
        {
            get { return UtilitaryFunctions.ToBitmapImage(pathToIcon); }
        }
        public string name { get; set; }
        public double health { get; set; }
        public double organization { get; set; }
        public double softAttack { get; set; }
        public double hardAttack { get; set; }
        public double defence { get; set; }
        public double breakthrough { get; set; }
        public double armor { get; set; }
        public double piercing { get; set; }
        public int frontWidth { get; set; }
        public double vehicleRatio { get; set; }
        public string pathToIcon { get; set; }

        public Unit()
        {
            name = string.Empty; health = 0; organization = 0; softAttack = 0; hardAttack = 0; defence = 0; breakthrough = 0; armor = 0; piercing = 0; frontWidth = 0; vehicleRatio = 0; pathToIcon = string.Empty;
        }
        public Unit(string name, string pathToIcon)
        {
            this.name = name; health = 0; organization = 0; softAttack = 0; hardAttack = 0; defence = 0; breakthrough = 0; armor = 0; piercing = 0; frontWidth = 0; vehicleRatio = 0; this.pathToIcon = pathToIcon;
        }
        public Unit(string name, double health, double organization, double softAttack, double hardAttack, double defence, double breakthrough, double armor, double piercing, int frontWidth, double vehicleRatio, string pathToIcon, List<TerrainModifier> modifiers)
        {
            this.name = name;
            this.health = health;
            this.organization = organization;
            this.softAttack = softAttack;
            this.hardAttack = hardAttack;
            this.defence = defence;
            this.breakthrough = breakthrough;
            this.armor = armor;
            this.piercing = piercing;
            this.frontWidth = frontWidth;
            this.vehicleRatio = vehicleRatio;
            this.pathToIcon = pathToIcon;
            this.modifiers = modifiers;
        }

        public List<TerrainModifier> modifiers = new()
        {
            new(TerrainType.forest, 1, 1),
            new(TerrainType.hill, 1, 1),
            new(TerrainType.mountains, 1, 1),
            new(TerrainType.plains, 1, 1),
            new(TerrainType.urban, 1, 1),
            new(TerrainType.jungle, 1, 1),
            new(TerrainType.marsh, 1, 1),
            new(TerrainType.desert, 1, 1),
            //new(TerrainType.river, 1, 1),
            //new(TerrainType.amphibious, 1, 1),
            //new(TerrainType.fort, 1, 1),
        };
        public override string ToString()
        {
            return name;
        }
    }
}
