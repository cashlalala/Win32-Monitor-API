using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Win32MultiMonitorDemo.Model;

namespace Win32MultiMonitorDemo.Util
{
    class WpfMonitorManager : IMonitorManager
    {
        public System.Collections.ObjectModel.ObservableCollection<Monitor> Monitors
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateMonitors(Rect? rect)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public Monitor GetMonitorCount(uint dwIndex)
        {
            throw new NotImplementedException();
        }

        public Monitor GetPrimaryMonitor()
        {
            throw new NotImplementedException();
        }

        public Monitor GetNearestMonitor(System.Windows.Rect rect)
        {
            throw new NotImplementedException();
        }

        public Monitor GetNearestMonitor(System.Windows.Point pt)
        {
            throw new NotImplementedException();
        }

        public Monitor GetNearestMonitor(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }
    }
}
