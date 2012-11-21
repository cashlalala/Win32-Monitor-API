using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Win32MultiMonitorDemo.Util;
using System.ComponentModel;
using System.Reflection;

namespace Win32MultiMonitorDemo.Model
{
    public class Win32StructWrapper
    {
        public class MonitorInfoEx : Win32.CMonitor.MONITORINFOEX, INotifyPropertyChanged
        {
            public MonitorInfoEx()
                : base()
            {}

            public override int CbSize
            {
                get { return _cbSize; }
                set 
                { 
                    _cbSize = value;
                    OnPropertyChanged("cbSize");
                }
            }

            public override Win32MultiMonitorDemo.Util.Win32.CMonitor.RECT rcMonitor
            {
                get { return _rcMonitor; }
                set 
                { 
                    _rcMonitor = value;
                    OnPropertyChanged("rcMonitor");
                }
            }

            public override Win32MultiMonitorDemo.Util.Win32.CMonitor.RECT rcWork
            {
                get { return _rcWork; }
                set 
                { 
                    _rcWork = value;
                    OnPropertyChanged("rcWork");
                }
            }

            public override int dwFlags
            {
                get { return _dwFlags; }
                set 
                { 
                    _dwFlags = value;
                    OnPropertyChanged("dwFlags");
                }
            }

            public override char[] szDevice
            {
                get { return _szDevice; }
                set 
                { 
                    _szDevice = value;
                    OnPropertyChanged("szDevice");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string property)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(property));
            }

            public Win32MultiMonitorDemo.Util.Win32.CMonitor.MONITORINFOEX copy2Win32Struct(MonitorInfoEx monitorInfoEx)
            {
                Win32MultiMonitorDemo.Util.Win32.CMonitor.MONITORINFOEX buf = new Win32MultiMonitorDemo.Util.Win32.CMonitor.MONITORINFOEX();
                foreach (PropertyInfo propInfo in monitorInfoEx.GetType().GetProperties())
                {
                    foreach (PropertyInfo propInfoInBuf in buf.GetType().GetProperties())
                    {
                        if (propInfo.Name == propInfoInBuf.Name)
                            propInfoInBuf.SetValue(buf, propInfo.GetValue(monitorInfoEx, null), null);
                    }
                }
                return buf;
            }

            public void copyFromWin32Struct(Win32MultiMonitorDemo.Util.Win32.CMonitor.MONITORINFOEX monitorInfoEx)
            {
                foreach (PropertyInfo propInfo in monitorInfoEx.GetType().GetProperties())
                {
                    foreach (PropertyInfo propInfoSelf in this.GetType().GetProperties())
                    {
                        if (propInfo.Name == propInfoSelf.Name)
                            propInfoSelf.SetValue(this, propInfo.GetValue(monitorInfoEx, null), null);
                    }
                }
            }
        }

    }
}
