using System;
using System.Globalization;
using System.Windows.Data;

namespace WonderStock.Converter
{
    public class FluctuationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fluctuation = (int)value;

            switch(fluctuation)
            {
                case 1: 
                case 2: return '+';
                case 4:
                case 5: return '-';
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
