using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class TerrainTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret;

            if (value is not TerrainType) ret = PathsToImages.NullGeneral;

            else
            {
                switch ((TerrainType)value)
                {
                    case TerrainType.forest:
                        {
                            ret = "./../../Images/Terrain/forest.png";
                            break;
                        }
                    case TerrainType.desert:
                        {
                            ret = "./../../Images/Terrain/desert.png";
                            break;
                        }
                    case TerrainType.hill:
                        {
                            ret = "./../../Images/Terrain/hills.png";
                            break;
                        }
                    case TerrainType.jungle:
                        {
                            ret = "./../../Images/Terrain/jungles.png";
                            break;
                        }
                    case TerrainType.marsh:
                        {
                            ret = "./../../Images/Terrain/marsh.png";
                            break;
                        }
                    case TerrainType.mountains:
                        {
                            ret = "./../../Images/Terrain/mountains.png";
                            break;
                        }
                    case TerrainType.plains:
                        {
                            ret = "./../../Images/Terrain/plains.png";
                            break;
                        }
                    default:
                        {
                            ret = "./../../Images/Terrain/urban.png";
                            break;
                        }
                }
            }

            return UtilitaryFunctions.ToBitmapImage(ret);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
