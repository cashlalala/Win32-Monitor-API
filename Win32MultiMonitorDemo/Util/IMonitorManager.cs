using System;
using System.Windows;
using System.Collections.ObjectModel;
using Win32MultiMonitorDemo.Model;

namespace Win32MultiMonitorDemo.Util
{
    public interface IMonitorManager
    {
        ObservableCollection<Monitor> Monitors
        {
            get;
            set;
        }

        void UpdateMonitors(Rect? rect);
        int GetCount();
        Monitor GetMonitorCount(uint dwIndex);
        Monitor GetPrimaryMonitor();
        Monitor GetNearestMonitor(Rect rect);
        Monitor GetNearestMonitor(Point pt);
        Monitor GetNearestMonitor(IntPtr hWnd);
    }
}
