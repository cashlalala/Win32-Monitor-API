using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Runtime.InteropServices;
using Win32MultiMonitorDemo.Model;

namespace Win32MultiMonitorDemo.Util
{
    class Win32MonitorManager : IMonitorManager
    {
        private ObservableCollection<Monitor> _monitors;

        public ObservableCollection<Monitor> Monitors
        {
            get
            {
                return _monitors;
            }
            set
            {
                _monitors = value;
            }
        }

        public Win32MonitorManager(Rect? rect = null)
        {
            this._monitors = new ObservableCollection<Monitor>();
            //this.UpdateMonitors(rect);
        }

#region Mehtod
        private bool EnumMonitorsCallBack(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, int dwData)
        {
// ReSharper disable RedundantThisQualifier
            this._monitors.Add(new Win32Monitor(hMonitor, (uint)this._monitors.Count));
// ReSharper restore RedundantThisQualifier
            return true;
        }


        public void UpdateMonitors(Rect? rcMonitor = null)
        {
            IntPtr rectPtr = IntPtr.Zero;
            try
            {
                this._monitors.Clear();
                if (rcMonitor.HasValue)
                {
                    var rect = new Win32Wrapper.CTypes.RECT(
                        Convert.ToInt32(rcMonitor.Value.Left), 
                        Convert.ToInt32(rcMonitor.Value.Top),
                        Convert.ToInt32(rcMonitor.Value.Right), 
                        Convert.ToInt32(rcMonitor.Value.Bottom));
                    rectPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Win32Wrapper.CTypes.RECT)));
                    Marshal.StructureToPtr(rect, rectPtr, true);
                }
                Win32Wrapper.CMonitor.EnumMonitorsDelegate dg = EnumMonitorsCallBack;
                Win32Wrapper.CMonitor.EnumDisplayMonitors(IntPtr.Zero, rectPtr, dg, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Get Win32 Error Code : {0}", Marshal.GetLastWin32Error()),
                                              String.Format("{0} : {1}", ex.Message,ex.StackTrace));
            }
            finally
            {
                if (rectPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(rectPtr);
            }

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

        public Monitor GetNearestMonitor(Rect rect)
        {
            throw new NotImplementedException();
        }

        public Monitor GetNearestMonitor(IntPtr hWnd)
        {
            throw new NotImplementedException();
        }

        public Monitor GetNearestMonitor(Point pt)
        {
            throw new Exception("The method or operation is not implemented.");
        }
#endregion




    }
}
