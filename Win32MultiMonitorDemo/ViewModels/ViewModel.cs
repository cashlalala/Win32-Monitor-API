using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Win32MultiMonitorDemo.Command;
using Win32MultiMonitorDemo.Util;

namespace Win32MultiMonitorDemo.ViewModels
{
    public class ViewModel
    {
        #region Field

        private readonly DispatcherTimer _dispatchTimer;

        private ICommand _startDetectCmd;

        private ICommand _stopDetectCmd;

        private ICommand _swithMachenismCmd;

        private IMonitorManager _monitorManager;

        private DeviceDetector _deviceDetector;

        #endregion

        #region Contructor

        public ViewModel(Window mainWin)
        {
            _dispatchTimer = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Normal, OnDispatcherTimer,
                                                 mainWin.Dispatcher);
            _dispatchTimer.Stop();;

            try
            {
                _monitorManager = MonitorManagerFactory.GetInstance();

                _monitorManager.UpdateMonitors(null);

                _deviceDetector = DeviceDetector.GetInstance();

                _deviceDetector.DeviceAttached += this.OnDeviceNotify;
                _deviceDetector.DeviceRemoved += this.OnDeviceNotify;
            }
            catch (Win32Exception win32Exception)
            {
                MessageBox.Show(String.Format("{0} \n StackTrace: \n{1}", win32Exception.Message, win32Exception.StackTrace),
                                             win32Exception.ErrorCode.ToString());
            }
            catch (System.TypeLoadException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Property

        public DeviceDetector DeviceDetector
        {
            get { return _deviceDetector; }
            set { _deviceDetector = value; }
        }

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

        public IMonitorManager MonitorManager
        {
            get { return _monitorManager; }
            set { _monitorManager = value; }
        }


        #endregion

        #region Methods

        private void OnDeviceNotify(object sender, DeviceDetector.DeviceChangedEventArgs e)
        {
            try
            {
                var deviceInfo = e.DeviceInfo as Win32Wrapper.CTypes.DEV_BROADCAST_DEVICEINTERFACE;
                if (deviceInfo != null)
                {
                    Guid deviceGuid = new Guid(deviceInfo.dbcc_classguid);
                    Guid monitorInterfaceGuid = new Guid("E6F07B5F-EE97-4a90-B076-33F57BF4EAA7");
                    if (deviceGuid.Equals(monitorInterfaceGuid))
                        _monitorManager.UpdateMonitors(null);
                }
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message, ex.ErrorCode.ToString());
            }

            
        }


        private void OnDispatcherTimer(object sender, EventArgs e)
        {
            //_monitorManager.UpdateMonitors(null);
        }


        private void StartDetect(object param)
        {
            _dispatchTimer.Start();
        }

        private void StopDect(object param)
        {
            _dispatchTimer.Stop();
        }

        public void OnHandleUpdated(object sender, EventArgs e)
        {
            DeviceDetector.ConfigTargetWindow(sender as Window);
        }

        private void SwitchDetectMachenism(object param)
        {
            var viewModel = (ViewModel) Convert.ChangeType(param, typeof (ViewModel));
            if (viewModel.MonitorManager is Win32MonitorManager)
                viewModel.MonitorManager = MonitorManagerFactory.GetInstance(MonitorManagerFactory.ManagerType.Wpf);
            else
                viewModel.MonitorManager = MonitorManagerFactory.GetInstance();
        }

        #endregion

    }
}