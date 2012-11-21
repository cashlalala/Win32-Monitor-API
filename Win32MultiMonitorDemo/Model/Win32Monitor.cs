using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Win32MultiMonitorDemo.Util;

namespace Win32MultiMonitorDemo.Model
{
    public class Win32Monitor : Monitor
    {
        public Win32Monitor()
        {}

        public Win32Monitor(IntPtr handle, uint index)
            : base(handle,index)
        {}

        public override System.Windows.Rect GetMonitorRect()
        {
            System.Windows.Rect rect = System.Windows.Rect.Empty;
            var monInfoEx = new Win32.CMonitor.MONITORINFOEX();
            bool succ = Win32.CMonitor.GetMonitorInfo(this.Handel, monInfoEx);
            if (succ)
                rect = new System.Windows.Rect(
                monInfoEx.rcMonitor.left, monInfoEx.rcMonitor.top, 
                Math.Abs(monInfoEx.rcMonitor.bottom - monInfoEx.rcMonitor.top), 
                Math.Abs(monInfoEx.rcMonitor.right-monInfoEx.rcMonitor.left));
            return rect;
        }

        public override System.Windows.Rect GetWorkAreaRect()
        {
            System.Windows.Rect rect = System.Windows.Rect.Empty;
            var monInfoEx = new Win32.CMonitor.MONITORINFOEX();
            bool succ = Win32.CMonitor.GetMonitorInfo(this.Handel, monInfoEx);
            if (succ)
                rect = new System.Windows.Rect(
                monInfoEx.rcWork.left, monInfoEx.rcWork.top,
                Math.Abs(monInfoEx.rcWork.bottom - monInfoEx.rcWork.top),
                Math.Abs(monInfoEx.rcWork.right - monInfoEx.rcWork.left));
            return rect;
        }
    }
}
