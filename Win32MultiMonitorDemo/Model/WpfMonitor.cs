using System;
using System.Windows;

namespace Win32MultiMonitorDemo.Model
{
    public class WpfMonitor : Monitor
    {
        public WpfMonitor() {}

        public WpfMonitor(IntPtr handle, uint index)
            : base(handle, index)
        {
            MonitorRect = new Rect(0, 10, 100, 100);
            WorkRect = new Rect(0, 10, 100, 100);
            Name = "fake";
        }
        public override System.Windows.Rect GetMonitorRect()
        {
            throw new System.NotImplementedException();
        }

        public override System.Windows.Rect GetWorkAreaRect()
        {
            throw new System.NotImplementedException();
        }
        
        public override void Update()
        {
            Console.WriteLine("update");
            MonitorRect = new Rect(50,50,150,200);
            WorkRect = new Rect(50, 50, 150, 200);
            Name = "Changed";
        }
    }
}