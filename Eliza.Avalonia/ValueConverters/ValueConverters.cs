﻿using Avalonia.Data;
using Avalonia.Data.Converters;
using Eliza.Avalonia.ViewModels;
using Eliza.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Eliza.Avalonia.ValueConverters
{
    // Need to declare these in App.axaml to expose them to views.

    // The direction is:
    // Convert means model type --> UI type
    // Convert Back: UI type --> model type



    // References:
    // https://docs.microsoft.com/en-us/dotnet/api/system.numerics.biginteger.-ctor?view=net-5.0

    public class PrimitiveConverter
    {
        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }


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
                val = Convert.ToBoolean((double)value % 2); // UI passes back as double
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
            }
            return val;
        }
    }

    public class UInt16ToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToUInt16(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UInt16 val = 0;
            try {
                val = UInt16.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }

    public class UInt32ToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToUInt32(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UInt32 val = 0;
            try {
                val = UInt32.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }

    public class UInt64ToStringConverter : PrimitiveConverter, IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToUInt64(value).ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            str = Regex.Replace(str, @"[^0-9]", "");
            if (str == "") {
                return 0;
            }
            BigInteger bigInt = BigInteger.Parse(str);
            UInt64 val;
            if      (bigInt > UInt64.MaxValue) { val = UInt64.MaxValue; }
            else if (bigInt < UInt64.MinValue) { val = UInt64.MinValue; }
            else { val = (UInt64)bigInt; }
            return val;
        }
    }

    public class UInt64ToHexConverter : PrimitiveConverter, IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToUInt64(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UInt64 val = 0;
            try {
                val = UInt64.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;

            /*
            UInt64 val;
            string hexStr = (string)value;
            hexStr = Regex.Replace(hexStr, @"[^0-9^A-F^a-f]", "");
            if (hexStr == "") {
                return 0;
            }
            if (hexStr.Length % 2 != 0) {
                hexStr = "0" + hexStr;
            }
            hexStr.PadLeft(8 * 2, '0');
            byte[] bytes = PrimitiveConverter.HexStringToByteArray(hexStr);
            if ((bytes[bytes.Length - 1] & 0x80) > 0) {
                byte[] temp = new byte[bytes.Length];
                Array.Copy(bytes, temp, bytes.Length);
                bytes = new byte[temp.Length + 1];
                Array.Copy(temp, bytes, temp.Length);
            }
            // Array.Reverse(bytes);
            BigInteger bigInt = new BigInteger(bytes);
            if (bigInt > UInt64.MaxValue) { val = UInt64.MaxValue; } else if (bigInt < UInt64.MinValue) { val = UInt64.MinValue; } else { val = (UInt64)bigInt; }
            return val;
            */
        }
    }

    public class SByteToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToSByte(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SByte val = 0;
            try {
                val = SByte.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }

    public class Int16ToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToInt16(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int16 val = 0;
            try {
                val = Int16.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }

    public class Int32ToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToInt32(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int32 val = 0;
            try {
                val = Int32.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }


    public class Int64ToStringConverter : PrimitiveConverter, IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert.ToInt64(value).ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            str = Regex.Replace(str, @"[^0-9]", "");
            if (str == "") {
                return 0;
            }
            BigInteger bigInt = BigInteger.Parse(str);
            Int64 val;
            if (bigInt > Int64.MaxValue) { val = Int64.MaxValue; } else if (bigInt < Int64.MinValue) { val = Int64.MinValue; } else { val = (Int64)bigInt; }
            return val;
        }
    }

    public class Int64ToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If source was int64 then this is fine.
            // Otherwise this can overflow if fed with just any double.
            return Convert.ToInt64(value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int64 val = 0;
            try {
                val = Int64.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return val;
        }
    }

    public class SingleToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value from UI is always double
            Single val = Convert.ToSingle(value);
            return BitConverter.SingleToInt32Bits(val).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int32 val = 0;
            try {
                val = Int32.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return BitConverter.Int32BitsToSingle(val);
        }
    }

    public class DoubleToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BitConverter.DoubleToInt64Bits((double)value).ToString("X");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int64 val = 0;
            try {
                val = Int64.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
            } catch (Exception) {
            }
            return BitConverter.Int64BitsToDouble(val);
        }
    }


    // Char conversions are nasty. It's only used in the header, so whatever works, works.
    // Force UI to ASCII to disable 2-byte entries e.g. Japanese characters.
    // ASCII is all we ever see in char fields i.e. RF5\0.
    // We could create dedicated widgets for these but there are so few of these fields that 
    // it's not worth the effort. Just let the exceptions happen.
    public class CharToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((char)value).ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            if(str.Length == 0) {
                #pragma warning disable CS8603 // Possible null reference return.
                return null;
                #pragma warning restore CS8603 // Possible null reference return.
            } else {
                return str[0];
            }
        }
    }

    public class CharToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning disable CS8604 // Possible null reference argument.
            try {
                string str = value.ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                return BitConverter.ToString(bytes).Replace("-", "");
            } catch (Exception) {
                return null;
            }
            #pragma warning restore CS8604 // Possible null reference argument.
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8603 // Possible null reference return.

        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try {
                string hex = (string)value;
                if (hex.Length % 2 == 0) {
                    int numChars = hex.Length;
                    byte[] bytes = new byte[numChars / 2];
                    for (int i = 0; i < numChars; i += 2)
                        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                    string str = Encoding.ASCII.GetString(bytes);
                    return str[0];
                } else {
                    #pragma warning disable CS8603 // Possible null reference return.
                    return null;
                    #pragma warning restore CS8603 // Possible null reference return.
                }
            } catch (Exception) {
                #pragma warning disable CS8603 // Possible null reference return.
                return null;
                #pragma warning restore CS8603 // Possible null reference return.
            }

        }
    }

    public class StringToHexConverter : IValueConverter
    {
        // https://stackoverflow.com/a/311179
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = Encoding.Unicode.GetBytes((string)value);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Pragmas are ugly but they're the best way to do it.
            // Convert() will handle the nullvalue exception and move on.
            try {
                string hex = (string)value;
                if(hex.Length % 4 == 0) {
                    int numChars = hex.Length;
                    byte[] bytes = new byte[numChars / 2];
                    for (int i = 0; i < numChars; i += 2)
                        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                    return Encoding.Unicode.GetString(bytes);
                } else {
                    #pragma warning disable CS8603 // Possible null reference return.
                    return null;
                    #pragma warning restore CS8603 // Possible null reference return.
                }
            } catch (Exception) {
                #pragma warning disable CS8603 // Possible null reference return.
                return null;
                #pragma warning restore CS8603 // Possible null reference return.
            }

        }
    }

    public class Utf8StringToHexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = Encoding.UTF8.GetBytes((string)value);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Pragmas are ugly but they're the best way to do it.
            // Convert() will handle the nullvalue exception and move on.
            try {
                string hex = (string)value;
                if (hex.Length % 2 == 0) {
                    int numChars = hex.Length;
                    byte[] bytes = new byte[numChars / 2];
                    for (int i = 0; i < numChars; i += 2)
                        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                    return Encoding.UTF8.GetString(bytes);
                } else {
                    #pragma warning disable CS8603 // Possible null reference return.
                    return null;
                    #pragma warning restore CS8603 // Possible null reference return.
                }
            } catch (Exception) {
                #pragma warning disable CS8603 // Possible null reference return.
                return null;
                #pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }


    public class IntToStringConverter : IValueConverter
    {
        // https://stackoverflow.com/a/311179
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null) {
                int val = (int)value;
                return val.ToString();
            } else {
                return "0";
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Pragmas are ugly but they're the best way to do it.
            // Convert() will handle the nullvalue exception and move on.
            try {
                if (value != null) {
                    int m = Int32.Parse((string)value);
                    if(m > Int32.MaxValue) {
                        m = Int32.MaxValue;
                    }
                    if(m < Int32.MinValue) {
                        m = Int32.MinValue;
                    }
                    return m;
                }
            } catch (Exception) {
            }
            return 0;
        }
    }

}