using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Win32MultiMonitorDemo.Command;
using Win32MultiMonitorDemo.Model;
using Win32MultiMonitorDemo.Util;

namespace Win32MultiMonitorDemo.ViewModels
{
    public class ViewModel
    {
        #region Field

        private readonly DispatcherTimer _dispatchTimer;
        private readonly WindowInteropHelper _winInterOpHelper;
        private IDetectMonitor _detectMonitor;

        private ICommand _startDetectCmd;

        private ICommand _stopDetectCmd;

        private ICommand _swithMachenismCmd;

        #endregion

        #region Contructor

        public ViewModel(Window mainWin)
        {
            _dispatchTimer = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Normal, OnDispatcherTimer,
                                                 mainWin.Dispatcher);
            _dispatchTimer.Stop();

            DetectMonitorMap = new Dictionary<DetectMethod, IDetectMonitor>
                {
                    {DetectMethod.Win32, new Win32Monitor()},
                    {DetectMethod.Wpf, new WpFmonitor()}
                };

            _detectMonitor = DetectMonitorMap[DetectMethod.Win32];

            _winInterOpHelper = new WindowInteropHelper(mainWin);

            Model = new Win32StructWrapper.MonitorInfoEx();
        }

        #endregion

        #region Property

        public Win32StructWrapper.MonitorInfoEx Model { get; set; }

        public Dictionary<DetectMethod, IDetectMonitor> DetectMonitorMap { get; set; }

        public ICommand SwithMachenismCmd
        {
            get { return _swithMachenismCmd ?? (_swithMachenismCmd = new RelayCommand(SwitchDetectMachenism)); }
            set { _swithMachenismCmd = value; }
        }

        public ICommand StartDetectCmd
        {
            get { return _startDetectCmd ?? (_startDetectCmd = new RelayCommand(StartDetect)); }
            set { _startDetectCmd = value; }
        }

        public ICommand StopDetectCmd
        {
            get { return _stopDetectCmd ?? (_stopDetectCmd = new RelayCommand(StopDect)); }
            set { _stopDetectCmd = value; }
        }

        #endregion

        #region Class

        private class Win32Monitor : IDetectMonitor
        {
            public void Detect(object param)
            {
                var viewModel = (ViewModel) Convert.ChangeType(param, typeof (ViewModel));

                var monInfoEx = new Win32.CMonitor.MONITORINFOEX();

                /*
                 * Get the monitor info from point
                 */
                //Win32.MultiMonitorHelper.POINTSTRUCT pt = new Win32.MultiMonitorHelper.POINTSTRUCT(2000, 500);
                //Win32.GeneralSafeHandle hMon = Win32.MultiMonitorHelper.MonitorFromPoint(pt, Win32.MultiMonitorHelper.MONITOR_DEFAULTTONEAREST);
                //bool result = Win32.MultiMonitorHelper.GetMonitorInfo(hMon, monInfoEx);

                /*
                 * Get the monitor info from window 
                 */
                IntPtr hMonFromWin = Win32.CMonitor.MonitorFromWindow(viewModel._winInterOpHelper.Handle,
                                                                      Win32.CMonitor.MONITOR_DEFAULTTONULL);

                bool result = Win32.CMonitor.GetMonitorInfo(hMonFromWin, monInfoEx);

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
        }

        private class WpFmonitor : IDetectMonitor
        {
            public void Detect(object param)
            {
                Console.WriteLine("QQorz");
            }
        }

        #endregion

        #region Methods

        private void OnDispatcherTimer(object sender, EventArgs e)
        {
            _detectMonitor.Detect(this);
        }


        private void StartDetect(object param)
        {
            _dispatchTimer.Start();
        }

        private void StopDect(object param)
        {
            _dispatchTimer.Stop();
        }

        private void SwitchDetectMachenism(object param)
        {
            var viewModel = (ViewModel) Convert.ChangeType(param, typeof (ViewModel));
            if (viewModel._detectMonitor is Win32Monitor)
                viewModel._detectMonitor = viewModel.DetectMonitorMap[DetectMethod.Wpf];
            else
                viewModel._detectMonitor = viewModel.DetectMonitorMap[DetectMethod.Win32];
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
            Win32 = 0,
            Wpf = 1
        };

        #endregion
    }
}