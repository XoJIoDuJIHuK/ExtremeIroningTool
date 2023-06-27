using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    public enum TerrainType
    {
        forest,
        hill,
        mountains,
        plains,
        urban,
        jungle,
        marsh,
        desert,
        river,
        amphibious,
        fort
    }
    public class TerrainModifier : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public TerrainType type { get; set; }
        public double attackModifier
        {
            get { return AttackModifier; }
            set { AttackModifier = value; OnPropertyChanged(); }
        }
        public double AttackModifier;
        public double defenseModifier
        {
            get { return DefenseModifier; }
            set { DefenseModifier = value; OnPropertyChanged(); }
        }
        public double DefenseModifier;
        public TerrainModifier(string strType, double attack, double defense)
        {
            switch (strType)
            {
                case "forest":
                    {
                        type = TerrainType.forest;
                        break;
                    }
                case "hill":
                    {
                        type = TerrainType.hill;
                        break;
                    }
                case "mountains":
                    {
                        type = TerrainType.mountains;
                        break;
                    }
                case "plains":
                    {
                        type = TerrainType.plains;
                        break;
                    }
                case "urban":
                    {
                        type = TerrainType.urban;
                        break;
                    }
                case "jungle":
                    {
                        type = TerrainType.jungle;
                        break;
                    }
                case "marsh":
                    {
                        type = TerrainType.marsh;
                        break;
                    }
                case "desert":
                    {
                        type = TerrainType.desert;
                        break;
                    }
                case "river":
                    {
                        type = TerrainType.river;
                        break;
                    }
                case "amphibious":
                    {
                        type = TerrainType.amphibious;
                        break;
                    }
                case "fort":
                    {
                        type = TerrainType.fort;
                        break;
                    }
                default:
                    {
                        type = TerrainType.plains;
                        break;
                    }
            }
            attackModifier = attack;
            defenseModifier = defense;
        }
        public TerrainModifier(TerrainType type, double attack, double defense)
        {
            this.type = type;
            attackModifier = attack;
            defenseModifier = defense;
        }
        public override string ToString()
        {
            return $"{type} {attackModifier} {defenseModifier}";
        }
    }
}
