using Avalonia.Data.Converters;
using Eliza.Avalonia.ViewModels;
using Eliza.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Avalonia.ValueConverters
{
    // Need to declare these in App.axaml to expose them to views.

    public class BoolToNumConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToByte(value);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean val;
            try {
                val = Convert.ToBoolean((double)value % 2);

            } catch (Exception) {

                val = false;
            }
            return val;
        }
    }

    public class ByteToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToByte(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte val = 0;
            try {
                val = byte.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {

                               // Do nothing. Return zero.
            }
            return val;
        }
    }


}