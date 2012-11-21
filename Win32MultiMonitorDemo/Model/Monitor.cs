using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Win32MultiMonitorDemo.Model
{
    public abstract class Monitor
    {
        

#region Contructor
        internal Monitor() { }
        
        internal Monitor(IntPtr handle, uint index)
        {
            this._handel = handle;
            this._Index = index;
        }
#endregion
        
#region Field
        private IntPtr _handel;

        private uint _Index;

        private string _name;
#endregion

#region Property
        public IntPtr Handel
        {
            get { return _handel; }
        }

        public uint Index
        {
            get { return _Index; }
        }

        public string Name
        {
            get { return _name; }
        }
#endregion

#region Abstract Method
        public abstract Rect GetMonitorRect();
        public abstract Rect GetWorkAreaRect();
#endregion
        
        public static explicit operator IntPtr(Monitor monitor)
        {
            return monitor._handel;
        }
    }
}
