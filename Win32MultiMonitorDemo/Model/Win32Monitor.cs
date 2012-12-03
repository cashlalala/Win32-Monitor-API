using System;
using System.Runtime.InteropServices;
using System.Windows;
using Win32MultiMonitorDemo.Util;

namespace Win32MultiMonitorDemo.Model
{
    public class Win32Monitor : Monitor
    {
        private Win32Wrapper.CMonitor.MONITORINFOEX monInfoEx;

        public Win32Monitor()
        {}

        public Win32Monitor(IntPtr handle, uint index)
            : base(handle, index)
        {
           monInfoEx = new Win32Wrapper.CMonitor.MONITORINFOEX();
            if (Win32Wrapper.CMonitor.GetMonitorInfo(handle, monInfoEx))
            {
                MonitorRect = new Rect(
                    monInfoEx.RcMonitor.left, monInfoEx.RcMonitor.top, 
                    Math.Abs(monInfoEx.RcMonitor.bottom - monInfoEx.RcMonitor.top), 
                    Math.Abs(monInfoEx.RcMonitor.right-monInfoEx.RcMonitor.left));
                WorkRect = new Rect(
                    monInfoEx.RcWork.left, monInfoEx.RcWork.top,
                    Math.Abs(monInfoEx.RcWork.bottom - monInfoEx.RcWork.top),
                    Math.Abs(monInfoEx.RcWork.right - monInfoEx.RcWork.left));
                Name = new string(monInfoEx.SzDevice);
            }
            else
            {
                String err = String.Format("Initial Fail: {0}", Marshal.GetLastWin32Error());
                throw new Exception(err);
            }
        }

        public override System.Windows.Rect GetMonitorRect()
        {
            var rect = System.Windows.Rect.Empty;
            bool succ = Win32Wrapper.CMonitor.GetMonitorInfo(this.Handle, monInfoEx);
            if (succ)
            {
                rect = new Rect(
                    monInfoEx.RcMonitor.left, monInfoEx.RcMonitor.top, 
                    Math.Abs(monInfoEx.RcMonitor.bottom - monInfoEx.RcMonitor.top), 
                    Math.Abs(monInfoEx.RcMonitor.right-monInfoEx.RcMonitor.left));
            }
            return rect;
        }

        public override System.Windows.Rect GetWorkAreaRect()
        {
            var rect = System.Windows.Rect.Empty;
            bool succ = Win32Wrapper.CMonitor.GetMonitorInfo(this.Handle, monInfoEx);
            if (succ)
            {
                rect = new Rect(
                    monInfoEx.RcWork.left, monInfoEx.RcWork.top,
                    Math.Abs(monInfoEx.RcWork.bottom - monInfoEx.RcWork.top),
                    Math.Abs(monInfoEx.RcWork.right - monInfoEx.RcWork.left));
            }  
            return rect;
        }

        public override void Update()
        {
            if (Win32Wrapper.CMonitor.GetMonitorInfo(Handle, monInfoEx))
            {
                WorkRect = new Rect(
                    monInfoEx.RcWork.left, monInfoEx.RcWork.top,
                    Math.Abs(monInfoEx.RcWork.bottom - monInfoEx.RcWork.top),
                    Math.Abs(monInfoEx.RcWork.right - monInfoEx.RcWork.left));

                MonitorRect = new Rect(
                    monInfoEx.RcMonitor.left, monInfoEx.RcMonitor.top,
                    Math.Abs(monInfoEx.RcMonitor.bottom - monInfoEx.RcMonitor.top),
                    Math.Abs(monInfoEx.RcMonitor.right - monInfoEx.RcMonitor.left));

                Name = new string(monInfoEx.SzDevice);
            }
        }
    }
}
