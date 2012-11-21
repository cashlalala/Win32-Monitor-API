﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Input;
using Win32MultiMonitorDemo.Util;
using System.Runtime.InteropServices;
using System.Windows;
using Win32MultiMonitorDemo.Command;
using Win32MultiMonitorDemo.Model;

namespace Win32MultiMonitorDemo.ViewModels
{
    public class ViewModel
    {
#region Field
        private DispatcherTimer dispatchTimer;
        
        private ICommand startDetectCmd;

        private ICommand stopDetectCmd;

        private ICommand swithMachenismCmd;

        private IDetectMonitor detectMonitor;

        private Dictionary<DetectMethod, IDetectMonitor> detectMonitorMap;

        private WindowInteropHelper winInterOpHelper;

        private Win32StructWrapper.MonitorInfoEx model;

#endregion

#region Contructor
        public ViewModel(MainWindow mainWin)
        {
            this.dispatchTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, this.OnDispatcherTimer, mainWin.Dispatcher);
            this.dispatchTimer.Stop();

            detectMonitorMap = new Dictionary<DetectMethod, IDetectMonitor>();
            detectMonitorMap.Add(DetectMethod.win32, new Win32Monitor());
            detectMonitorMap.Add(DetectMethod.wpf, new WPFmonitor());

            this.detectMonitor = detectMonitorMap[DetectMethod.win32];

            this.winInterOpHelper = new WindowInteropHelper(mainWin);

            this.model = new Win32StructWrapper.MonitorInfoEx();
        }
#endregion

#region Property
        public Win32StructWrapper.MonitorInfoEx Model
        {
            get { return model; }
            set { model = value; }
        }
        public Dictionary<DetectMethod, IDetectMonitor> DetectMonitorMap
        {
            get { return detectMonitorMap; }
            set { detectMonitorMap = value; }
        }

        public ICommand SwithMachenismCmd
        {
            get
            {
                if (this.swithMachenismCmd == null)
                    this.swithMachenismCmd = new RelayCommand(SwitchDetectMachenism);
                return swithMachenismCmd;
            }
            set { swithMachenismCmd = value; }
        }

        public ICommand StartDetectCmd
        {
            get
            {
                if (this.startDetectCmd == null)
                    this.startDetectCmd = new RelayCommand(StartDetect);
                return startDetectCmd;
            }
            set { startDetectCmd = value; }
        }

        public ICommand StopDetectCmd
        {
            get
            {
                if (this.stopDetectCmd == null)
                    this.stopDetectCmd = new RelayCommand(StopDect);
                return stopDetectCmd;
            }
            set { stopDetectCmd = value; }
        }


#endregion


#region Class
        private class Win32Monitor : IDetectMonitor
        {
            public void Detect(object param)
            {
                ViewModel viewModel = (ViewModel)Convert.ChangeType(param, typeof(ViewModel));

                Win32.MultiMonitor.MONITORINFOEX monInfoEx = new Win32.MultiMonitor.MONITORINFOEX();

                /*
                 * Get the monitor info from point
                 */
                //Win32.MultiMonitorHelper.POINTSTRUCT pt = new Win32.MultiMonitorHelper.POINTSTRUCT(2000, 500);
                //Win32.GeneralSafeHandle hMon = Win32.MultiMonitorHelper.MonitorFromPoint(pt, Win32.MultiMonitorHelper.MONITOR_DEFAULTTONEAREST);
                //bool result = Win32.MultiMonitorHelper.GetMonitorInfo(hMon, monInfoEx);

                /*
                 * Get the monitor info from window 
                 */
                IntPtr hMonFromWin = Win32.MultiMonitor.MonitorFromWindow(viewModel.winInterOpHelper.Handle, Win32.MultiMonitor.MONITOR_DEFAULTTONEAREST);

                bool result = Win32.MultiMonitor.GetMonitorInfo(hMonFromWin, monInfoEx);

                if (result)
                    viewModel.Model.copyFromWin32Struct(monInfoEx);
                else
                    MessageBox.Show(String.Format("Get error code: {0}", Marshal.GetLastWin32Error()));
            }

            //private static bool enumDesktop(string desktopName, int lParam)
            //{
            //    //this.monInfo.monWidthHeight.Text = String.Format("DesktopName: {0:},\t lParam: {1}\r\n", desktopName, lParam);
            //    return true;
            //}
            
            public Win32Monitor()
            {
                // TODO: Complete member initialization
            }
        }

        public class WPFmonitor : IDetectMonitor
        {

            public void Detect(object param)
            {
                Console.WriteLine("QQorz");
            }

            public WPFmonitor()
            {

            }
        }
#endregion

#region Methods

        private void OnDispatcherTimer(object sender, EventArgs e)
        {
            this.detectMonitor.Detect(this);
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.dispatchTimer.IsEnabled)
            {
                this.dispatchTimer.Stop();
            }
        }

        private void StartDetect(object param)
        {
            this.dispatchTimer.Start();
        }

        private void StopDect(object param)
        {
            this.dispatchTimer.Stop();
        }

        private void SwitchDetectMachenism(object param)
        {
            ViewModel viewModel = (ViewModel)Convert.ChangeType(param, typeof(ViewModel));
            if (viewModel.detectMonitor is Win32Monitor)
                viewModel.detectMonitor = viewModel.detectMonitorMap[DetectMethod.wpf];
            else
                viewModel.detectMonitor = viewModel.detectMonitorMap[DetectMethod.win32];
        }
#endregion

#region Interface
        public interface IDetectMonitor
        {
            void Detect(object param);
        }

#endregion

#region Enum
        public enum DetectMethod
        {
            win32 = 0,
            wpf = 1
        };
#endregion
    }
}