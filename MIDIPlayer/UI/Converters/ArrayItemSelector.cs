using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hscm.UI
{
    public class ArrayItemSelector : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Check given arguments first
            if (!(values.Length > 1) || !(values[0] is string[]) || !(values[1] is int))
                throw new ArgumentException("given values not correct");

            return ((string[])values[0])[(int)values[1]];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
