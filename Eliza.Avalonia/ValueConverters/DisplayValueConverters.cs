using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Eliza.Avalonia.ViewModels;
using Eliza.Core.Serialization;
using Eliza.Data;
using Eliza.Model.Item;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eliza.Avalonia.ValueConverters
{
    // Be careful when renaming these. AXAML files will not be updated.



    // These are one-way value converters for display-only values.



    // Input is UiObjectGraph. If node.Child[0].ItemId is int, return item's name as string.

    public class DisplayUiObjectGraphItemIdToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() != typeof(UnsetValueType)) {
                try {
                    UiObjectGraph uiNode = (UiObjectGraph)value;
                    Type? type = uiNode.Type;
                    FieldInfo? fieldInfo = uiNode.FieldInfo;
                    object? nodeVal = uiNode.Value;
                    if (type == typeof(int) && fieldInfo != null && nodeVal != null) {
                        int val = (int)nodeVal;
                        string fieldName = fieldInfo.Name;
                        if (fieldName == "ItemId") {
                            string str = Items.ItemIdToAssemblyResourceUri[val];
                            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                            var asset = assets.Open(new Uri(str));
                            return new Bitmap(asset);
                        }
                    }
                } catch (Exception) {
                    // This converter is called potentially thousands of times.
                    // Make sure no exceptions are thrown.
                }
            }
            return "";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Hides item-related ui elements if context is not an item. Returns true/false.
    public class DisplayUiObjectGraphItemIdToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() != typeof(UnsetValueType)) {
                try {
                    UiObjectGraph uiNode = (UiObjectGraph)value;
                    Type? type = uiNode.Type;
                    FieldInfo? fieldInfo = uiNode.FieldInfo;
                    object? nodeVal = uiNode.Value;
                    if (type == typeof(int) && fieldInfo != null && nodeVal != null) {
                        int val = (int)nodeVal;
                        string fieldName = fieldInfo.Name;
                        if (fieldName == "ItemId") {
                            return true;
                        }
                    }
                } catch (Exception) {
                    // This converter is called potentially thousands of times.
                    // Make sure no exceptions are thrown.
                }
            }
            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }





    /// <summary>
    /// Takes an instance of UiObjectGraph and returns its pretty-printed field name. Used in treeview.
    /// This is not a dynamic property since it binds to .This
    /// </summary>
    public class DisplayArrayIndexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null) {
                int val = (int)value;
                if(val != ObjectGraph.NULL_ARRAY_INDEX) {
                    return val.ToString();
                }
            }
            return "";
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


    public class ItemIdToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = 0;
            try {
                if (value != null && value != AvaloniaProperty.UnsetValue && value.GetType().IsPrimitive) {
                    val = (int)value;
                    if(! Items.ItemIds.Contains(val)) {
                        val = 0;
                    }
                }
            } catch (Exception) {
                // Do nothing
            }
            string str = Items.ItemIdToAssemblyResourceUri[val];
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var asset = assets.Open(new Uri(str));
            return new Bitmap(asset);

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemIdToJapaneseNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = 0;
            try {
                if (value != null && value != AvaloniaProperty.UnsetValue && value.GetType().IsPrimitive) {
                    val = (int)value;
                    if (!Items.ItemIds.Contains(val)) {
                        val = 0;
                    }
                }
            } catch (Exception e) {
                // Do nothing
            }
            return Items.ItemIdToJapaneseName[val];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemIdToEnglishNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = 0;
            try {
                if (value != null && value != AvaloniaProperty.UnsetValue && value.GetType().IsPrimitive) {
                    val = (int)value;
                    if (!Items.ItemIds.Contains(val)) {
                        val = 0;
                    }
                }
            } catch (Exception e) {
                // Do nothing
            }
            return Items.ItemIdToEnglishName[val];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemIdToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = 0;
            try {
                if (value != null && value != AvaloniaProperty.UnsetValue && value.GetType().IsPrimitive) {
                    val = (int)value;
                    if (!Items.ItemIds.Contains(val)) {
                        val = 0;
                    }
                }
            } catch (Exception e) {
                // Do nothing
            }
            return (val == 0) ? false : true;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TypeToTreeViewVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = false;
            try {
                if (value != null && value != AvaloniaProperty.UnsetValue) {
                    Type type = (Type)value;
                    if ((type == typeof(ItemData))
                        || (type == typeof(AmountItemData))
                        || (type == typeof(SeedItemData))
                        || (type == typeof(EquipItemData))
                        || (type == typeof(FishItemData))
                        || (type == typeof(FoodItemData))
                        || (type == typeof(PotToolItemData))
                        || (type == typeof(RuneAbilityItemData))) {
                        val = true;
                    }
                }
            } catch (Exception) {
                // Do nothing
            }
            return val;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




}
