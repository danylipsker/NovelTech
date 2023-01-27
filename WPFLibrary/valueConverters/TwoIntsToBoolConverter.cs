using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WPFLibrary.valueConverters
{
    public class TwoIntsToBoolConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>0 = Equal | 1 = First > Second | -1 First < Second</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length == 2 && values[0] is int && values[1] is int)
            {
                if ((int)values[0] == (int)values[1])
                    return 0;
                if ((int)values[0] > (int)values[1])
                    return 1;
            }
            return -1;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
