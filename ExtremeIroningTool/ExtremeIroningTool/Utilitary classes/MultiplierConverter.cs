using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class MultiplierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = (double)value;
            var strParam = (string)parameter;
            string temp = string.Empty;
            for (int i = 0; i < strParam.Length; i++)
            {
                if (strParam[i] == '.') temp += ',';
                else temp += strParam[i];
            }
            strParam = temp;
            if (double.TryParse(strParam, out double param)) return height * param;
            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
