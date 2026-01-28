using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfPaint.Views.Converters
{
    public class PointToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point point && parameter is string propertyName)
            {
                if (propertyName == "X")
                {
                    return point.X;
                }
                else if (propertyName == "Y")
                {
                    return point.Y;
                }
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
