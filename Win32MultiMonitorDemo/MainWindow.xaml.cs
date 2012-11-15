using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Win32MultiMonitorDemo.Util;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Threading;

using System.Windows.Interop;

namespace Win32MultiMonitorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            winHelper = new WindowInteropHelper(this);

            this.dispatchTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, OnDispatcherTimer, this.Dispatcher);

        }

        private DispatcherTimer dispatchTimer;

        private WindowInteropHelper winHelper;

        private static int count = 0;

        private void OnDispatcherTimer(object sender, EventArgs e)
        {
            Win32.MultiMonitorHelper.MONITORINFOEX monInfoEx = new Win32.MultiMonitorHelper.MONITORINFOEX();
           
            /*
             * Get the monitor info from point
             */
            //Win32.MultiMonitorHelper.POINTSTRUCT pt = new Win32.MultiMonitorHelper.POINTSTRUCT(2000, 500);
            //Win32.GeneralSafeHandle hMon = Win32.MultiMonitorHelper.MonitorFromPoint(pt, Win32.MultiMonitorHelper.MONITOR_DEFAULTTONEAREST);
            //bool result = Win32.MultiMonitorHelper.GetMonitorInfo(hMon, monInfoEx);

            /*
             * Get the monitor info from window 
             */
            Win32.GeneralSafeHandle hMonFromWin = Win32.MultiMonitorHelper.MonitorFromWindow(winHelper.Handle, Win32.MultiMonitorHelper.MONITOR_DEFAULTTONEAREST);

            bool result = Win32.MultiMonitorHelper.GetMonitorInfo(hMonFromWin, monInfoEx);


            if (result)
            {
                this.monInfo.a1.Text = new String(monInfoEx.szDevice).TrimEnd('\0') + (++count).ToString();
                this.monInfo.a2.Text = String.Format("handle in winhelp : {0:X}",winHelper.Handle);
                //this.monInfo.a3.Text = String.Format("handle in windowHandle : {0:X}", );
            }
            else
            {
                //throw new Win32Exception(Marshal.GetLastWin32Error());
                this.monInfo.a1.Text = String.Format("Get error code: {0}", Marshal.GetLastWin32Error());
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.dispatchTimer.IsEnabled)
            {
                this.dispatchTimer.Stop();
            }
        }
    }


}
