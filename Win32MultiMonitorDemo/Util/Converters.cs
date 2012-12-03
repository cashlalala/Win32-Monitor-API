using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Win32MultiMonitorDemo.Util
{
    public class CharAryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is char[])
                return new String(value as char[]);
            else
                return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
                return (value as String).ToCharArray();
            else
                return new char[]{};
        }
    }

    public class Int2RectConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Rect)
            {
                value = (Rect)value;
                return value.ToString();
            }  
            else
                return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                String[] result = (value as String).Split(new[] {',',' ','\0',':',
                                                                                            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                                                            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'});
                var rect = new Rect(double.Parse(result[0]),double.Parse(result[1]),
                    double.Parse(result[2]),double.Parse(result[3]));
                return rect;
            }  
            else
            {
                return new Win32Wrapper.CTypes.RECT();
            }
                
        }
    }
}
