using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CFM.AssignmentModule
{
    public class DaysLeftToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int days = System.Convert.ToInt32(value);
            if (days > 0 && days <= 3)
                return Colors.Red;
            if (days > 3 && days <= 5)
                return Colors.DarkOrange;
            if (days > 5 && days <= 10)
                return Colors.Yellow;
            return Colors.LawnGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}