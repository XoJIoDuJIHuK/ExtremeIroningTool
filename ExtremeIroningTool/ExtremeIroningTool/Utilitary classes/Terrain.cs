using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class Terrain
    {
        public TerrainType type;
        public double attackModifier;
        public byte combatWidth;
        public string PathToIcon { get; set; }

        string GetPathToIcon(TerrainType t)
        {
            switch (t)
            {
                case TerrainType.desert:
                    {
                        return "./../../Images/Terrain/desert.png";
                    }
                case TerrainType.forest:
                    {
                        return "./../../Images/Terrain/forest.png";
                    }
                case TerrainType.hill:
                    {
                        return "./../../Images/Terrain/hills.png";
                    }
                case TerrainType.jungle:
                    {
                        return "./../../Images/Terrain/jungles.png";
                    }
                case TerrainType.marsh:
                    {
                        return "./../../Images/Terrain/marsh.png";
                    }
                case TerrainType.mountains:
                    {
                        return "./../../Images/Terrain/mountains.png";
                    }
                case TerrainType.plains:
                    {
                        return "./../../Images/Terrain/plains.png";
                    }
                default:
                    {
                        return "./../../Images/Terrain/urban.png";
                    }
            }
        }

        public Terrain(TerrainType type, double attackModifier, byte combatWidth)
        {
            this.type = type;
            this.attackModifier = attackModifier;
            this.combatWidth = combatWidth;
            PathToIcon = GetPathToIcon(type);
        }

        public Terrain(string type, double attackModifier, byte combatWidth)
        {
            switch (type)
            {
                case "forest":
                    {
                        this.type = TerrainType.forest;
                        break;
                    }
                case "hills":
                    {
                        this.type = TerrainType.hill;
                        break;
                    }
                case "mountains":
                    {
                        this.type = TerrainType.mountains;
                        break;
                    }
                case "plains":
                    {
                        this.type = TerrainType.plains;
                        break;
                    }
                case "urban":
                    {
                        this.type = TerrainType.urban;
                        break;
                    }
                case "jungles":
                    {
                        this.type = TerrainType.jungle;
                        break;
                    }
                case "marshes":
                    {
                        this.type = TerrainType.marsh;
                        break;
                    }
                case "desert":
                    {
                        this.type = TerrainType.desert;
                        break;
                    }
                default:
                    {
                        this.type = TerrainType.plains;
                        break;
                    }
            }

            this.attackModifier = attackModifier;
            this.combatWidth = combatWidth;
            PathToIcon = GetPathToIcon(this.type);
        }
    }
}
