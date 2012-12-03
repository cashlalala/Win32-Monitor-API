using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Win32MultiMonitorDemo.Model;

namespace Win32MultiMonitorDemo.Util
{
    class WpfMonitorManager : IMonitorManager
    {
        private ObservableCollection<Monitor> _monitors;

        public System.Collections.ObjectModel.ObservableCollection<Monitor> Monitors
        {
            get { return _monitors; }
            set { _monitors = value; }
        }

        public WpfMonitorManager(Rect? rect = null)
        {
            this.UpdateMonitors(rect);
        }

        public void UpdateMonitors(Rect? rect)
        {
            Console.WriteLine("QQorz");
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
