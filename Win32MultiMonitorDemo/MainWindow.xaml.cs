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
using Win32MultiMonitorDemo.Command;

namespace Win32MultiMonitorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

#region Fields

        private DispatcherTimer dispatchTimer;

        private WindowInteropHelper winHelper;

        private static int count = 0;

        private ICommand startDetectCmd;

        private ICommand stopDetectCmd;

        private ICommand swithMachenismCmd;

        private IDetectMonitor detectMonitor;

        private Dictionary<DetectMethod, IDetectMonitor> detectMonitorMap;
#endregion

#region Property

        public System.Windows.Threading.DispatcherTimer DispatchTimer
        {
            get { return dispatchTimer; }
            set { dispatchTimer = value; }
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

#region Constructors
        public MainWindow()
        {
            InitializeComponent();

            winHelper = new WindowInteropHelper(this);

            this.dispatchTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, OnDispatcherTimer, this.Dispatcher);
            this.dispatchTimer.Stop();

            detectMonitorMap = new Dictionary<DetectMethod, IDetectMonitor>();
            detectMonitorMap.Add(DetectMethod.win32, new Win32Monitor());
            detectMonitorMap.Add(DetectMethod.wpf, new WPFmonitor());

            this.detectMonitor = detectMonitorMap[DetectMethod.win32];
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
            MainWindow mainWin = (MainWindow) Convert.ChangeType(param,typeof(MainWindow));
            if (mainWin.detectMonitor is Win32Monitor)
                mainWin.detectMonitor = mainWin.detectMonitorMap[DetectMethod.wpf];
            else
                mainWin.detectMonitor = mainWin.detectMonitorMap[DetectMethod.win32];
        }
#endregion

#region Interface
        private interface IDetectMonitor
        {
            void Detect(object param);
        }

#endregion

#region Enum
        public enum DetectMethod
        {
            win32 = 0,
            wpf =1
        };
#endregion

#region Class
        private class Win32Monitor : IDetectMonitor
        {
            public void Detect(object param)
            {
                MainWindow mainWin = (MainWindow)Convert.ChangeType(param, typeof(MainWindow));

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
                IntPtr hMonFromWin = Win32.MultiMonitor.MonitorFromWindow(mainWin.winHelper.Handle, Win32.MultiMonitor.MONITOR_DEFAULTTONEAREST);

                bool result = Win32.MultiMonitor.GetMonitorInfo(hMonFromWin, monInfoEx);


                if (result)
                {
                    mainWin.monInfo.monitorNo.Text = new String(monInfoEx.szDevice).TrimEnd('\0') + (++count).ToString();
                    mainWin.monInfo
                    mainWin.monInfo.winLeftTopPointInMon.Text = String.Format("handle in winhelp : {0:X}", mainWin.winHelper.Handle);
                    //Win32.Display.EnumDesktopsDelegate pro = enumDesktop;
                    //Win32.SafeWindowStationHandle safeHandle = Win32.Display.GetProcessWindowStation();
                    //IntPtr lParam = IntPtr.Zero;
                    //if (!Win32.Display.EnumDesktops(safeHandle.DangerousGetHandle(), pro, lParam))
                    //{

                    //}
                }
                else
                {
                    MessageBox.Show(String.Format("Get error code: {0}", Marshal.GetLastWin32Error()));
                }
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

        private class WPFmonitor : IDetectMonitor
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
    }


}
