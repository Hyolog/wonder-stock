using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WonderStock.Converter
{
    public class FluctutationColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fluctuation = (int)value;

            switch (fluctuation)
            {
                case 1:
                case 2: return Brushes.Red;
                case 4:
                case 5: return Brushes.Blue;
                default: return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
