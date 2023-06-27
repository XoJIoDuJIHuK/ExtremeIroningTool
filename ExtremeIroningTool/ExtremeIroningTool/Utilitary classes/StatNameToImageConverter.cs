using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class StatNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret;
            switch (value as string)
            {
                case "Health":
                    {
                        ret = PathsToImages.HealthIcon;
                        break;
                    }
                case "Organization":
                    {
                        ret = PathsToImages.OrganizationIcon;
                        break;
                    }
                case "Soft attack":
                    {
                        ret = PathsToImages.SoftAttackIcon;
                        break;
                    }
                case "Hard attack":
                    {
                        ret = PathsToImages.HardAttackIcon;
                        break;
                    }
                case "Defense":
                    {
                        ret = PathsToImages.DefenseIcon;
                        break;
                    }
                case "Breakthrough":
                    {
                        ret = PathsToImages.BreakthroughIcon;
                        break;
                    }
                case "Armor":
                    {
                        ret = PathsToImages.ArmorIcon;
                        break;
                    }
                case "Piercing":
                    {
                        ret = PathsToImages.PiercingIcon;
                        break;
                    }
                case "Front width":
                    {
                        ret = PathsToImages.FrontWidthIcon;
                        break;
                    }
                default:
                    {
                        ret = PathsToImages.VehicleRatioIcon;
                        break;
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
