using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (value is Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT)
            {
                value = (Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT) value;
                return value.ToString();
            }  
            else
                return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                String[] result = (value as String).Split(new char[] {',',' ','\0',':',
                                                                                            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                                                            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'});
                Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT rect = new Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT();
                rect.left = int.Parse(result[0]);
                rect.top = int.Parse(result[1]);
                rect.right = int.Parse(result[2]);
                rect.bottom = int.Parse(result[3]);
                return rect;
            }  
            else
            {
                return new Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT();
            }
                
        }
    }
}
