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
    // Be careful when renaming these. AXAML files will not be updated.



    // These are one-way value converters for display-only values.

    /// <summary>
    /// Takes an instance of UiObjectGraph and returns its pretty-printed field name. Used in treeview.
    /// This is not a dynamic property since it binds to .This
    /// </summary>
    public class DisplayTypeNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UiObjectGraph val = (UiObjectGraph)value;
            string str = val.Type.Name;
            if (val.ArrayIndex != ObjectGraph.NULL_ARRAY_INDEX) {
                str += " " + val.ArrayIndex.ToString();
            }
            return str;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Takes an instance of UiObjectGraph and returns whether its value is displayed in treeview.
    /// This is not a dynamic property since it binds to .This
    /// </summary>
    public class VisibilityValueConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UiObjectGraph val = (UiObjectGraph)value;
            if (val.Type == typeof(string)) {
                return true;
            } else if (val.Type == null || !val.Type.IsPrimitive) {
                return false;
            } else {
                return true;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
