using Win32MultiMonitorDemo.Util;
using System.ComponentModel;
using System.Reflection;

namespace Win32MultiMonitorDemo.Model
{
    public class Win32StructWrapper
    {
        public class MonitorInfoEx : Win32.CMonitor.MONITORINFOEX, INotifyPropertyChanged
        {
            public override int CbSize
            {
                get { return CbSize; }
                set 
                { 
                    CbSize = value;
                    OnPropertyChanged("cbSize");
                }
            }

            public override Win32MultiMonitorDemo.Util.Win32.CMonitor.RECT RcMonitor
            {
                get { return RcMonitor; }
                set 
                { 
                    RcMonitor = value;
                    OnPropertyChanged("rcMonitor");
                }
            }

            public override Win32MultiMonitorDemo.Util.Win32.CMonitor.RECT RcWork
            {
                get { return RcWork; }
                set 
                { 
                    RcWork = value;
                    OnPropertyChanged("rcWork");
                }
            }

            public override int DwFlags
            {
                get { return DwFlags; }
                set 
                { 
                    DwFlags = value;
                    OnPropertyChanged("dwFlags");
                }
            }

            public override char[] SzDevice
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

            public Win32.CMonitor.MONITORINFOEX Copy2Win32Struct(MonitorInfoEx monitorInfoEx)
            {
                var buf = new Win32.CMonitor.MONITORINFOEX();
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

            public void CopyFromWin32Struct(Win32.CMonitor.MONITORINFOEX monitorInfoEx)
            {
                foreach (PropertyInfo propInfo in monitorInfoEx.GetType().GetProperties())
                {
                    foreach (PropertyInfo propInfoSelf in GetType().GetProperties())
                    {
                        if (propInfo.Name == propInfoSelf.Name)
                            propInfoSelf.SetValue(this, propInfo.GetValue(monitorInfoEx, null), null);
                    }
                }
            }
        }

    }
}
