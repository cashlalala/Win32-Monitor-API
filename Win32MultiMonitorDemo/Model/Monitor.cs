using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Win32MultiMonitorDemo.Model
{
    public abstract class Monitor : INotifyPropertyChanged
    {
        

#region Contructor
        internal Monitor() { }
        
        internal Monitor(IntPtr handle, uint index)
        {
            this._handle = handle;
            this._index = index;
            _name = String.Empty;
        }
#endregion
        
#region Field

        private Rect _monitorRect;

        private Rect _workRect;

        private IntPtr _handle;

        private uint _index;

        private string _name;
#endregion

#region Property
        public IntPtr Handle
        {
            get { return _handle; }
        }

        public uint Index
        {
            get { return _index; }
        }

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Rect MonitorRect
        {
            get { return _monitorRect; }
            set
            {
                _monitorRect = value;
                OnPropertyChanged("MonitorRect");
            }
        }

        public Rect WorkRect
        {
            get { return _workRect; }
            set
            {
                _workRect = value;
                OnPropertyChanged("WorkRect");
            }
        }

        #endregion

#region Abstract Method
        public abstract Rect GetMonitorRect();
        public abstract Rect GetWorkAreaRect();
        public abstract void Update();
#endregion
        
        public static explicit operator IntPtr(Monitor monitor)
        {
            return monitor._handle;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }
    }
}
