using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using log4net;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Win32MultiMonitorDemo.Util
{
    public class DeviceDetector
    {

        #region Class
        
        private class WindowKeeper
        {
            public WindowKeeper(Window window)
            {
                Window = window;
                Handle = (new WindowInteropHelper(Window)).EnsureHandle();
            }

            public Window Window { get; set; }

            public IntPtr Handle { get; set; }
        }

        /// <summary>
        /// Class for passing in custom arguments to our event handlers.
        /// </summary>
        public class DeviceChangedEventArgs : EventArgs
        {
            private static readonly ILog logger = LogManager.GetLogger(typeof(DeviceChangedEventArgs).Name);

            private readonly IntPtr _deviceInfoHandle;

            private object _deviceInfo;

            public DeviceChangedEventArgs(IntPtr deviceInfoHandle)
            {
                this._deviceInfoHandle = deviceInfoHandle;

                var devType = (Win32Wrapper.CTypes.DBTDEVTYP)Marshal.ReadIntPtr(_deviceInfoHandle, 4);
                logger.DebugFormat("Device Type: [{0:x}]", devType);
                switch (devType)
                {
                    case Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE:
                        _deviceInfo = Marshal.PtrToStructure(_deviceInfoHandle, typeof(Win32Wrapper.CTypes.DEV_BROADCAST_DEVICEINTERFACE));
                        break;
                    case Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_HANDLE:
                        _deviceInfo = Marshal.PtrToStructure(_deviceInfoHandle, typeof(Win32Wrapper.CTypes.DEV_BROADCAST_HANDLE));
                        break;
                    case Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_OEM:
                        _deviceInfo = Marshal.PtrToStructure(_deviceInfoHandle, typeof(Win32Wrapper.CTypes.DEV_BROADCAST_OEM));
                        break;
                    case Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_PORT:
                        _deviceInfo = Marshal.PtrToStructure(_deviceInfoHandle, typeof(Win32Wrapper.CTypes.DEV_BROADCAST_PORT));
                        break;
                    case Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_VOLUME:
                        _deviceInfo = Marshal.PtrToStructure(_deviceInfoHandle, typeof(Win32Wrapper.CTypes.DEV_BROADCAST_VOLUME));
                        break;
                    default:
                        _deviceInfo = new Win32Wrapper.CTypes.DEV_BROADCAST_HDR();
                        logger.DebugFormat("Unknown Device Type: [{0:x}]", devType);
                        break;
                }
            }

            public object DeviceInfo
            {
                get
                {
                    return _deviceInfo;
                }
            }

            public override string ToString()
            {
                //var _deviceInfo = (Win32Wrapper.CTypes.DEV_BROADCAST_HDR) this._deviceInfo;
                if (_deviceInfo is Win32Wrapper.CTypes.DEV_BROADCAST_DEVICEINTERFACE)
                {
                    var temp = (Win32Wrapper.CTypes.DEV_BROADCAST_DEVICEINTERFACE)this._deviceInfo;
                    return String.Format("size:{0}, type: {1}, reserved: {2}, name:{3}", temp.dbch_size,
                                         temp.dbch_devicetype, temp.dbch_reserved, new string(temp.dbcc_name));
                }
                else
                {
                    var temp = (Win32Wrapper.CTypes.DEV_BROADCAST_HDR)this._deviceInfo;
                    return String.Format("size:{0}, type: {1}, reserved: {2}", temp.dbch_size, temp.dbch_devicetype, temp.dbch_reserved);
                }

            }
        }
        #endregion

        #region Field

        private static readonly ILog Logger = LogManager.GetLogger(typeof(DeviceDetector).Name);

        private const int BufferSize = 1024;

        private WindowKeeper _windowKeeper;

        private SafeDeviceHandle _interfaceNotificationHandle;

        private bool _isWindowConfiged;

        private static DeviceDetector _deviceDetector;

        #endregion

        #region Event & Delegate

        /// <summary>
        /// Events signalized to the client app.
        /// Add handlers for these events in your form to be notified of removable device events.
        /// </summary>
        public event DeviceEventHandler DeviceAttached;
        public event EventHandler<DeviceChangedEventArgs> DeviceRemoved;
        //public event DeviceEventHandler DeviceQueryRemove;

        //Delegate for event handler to handle the device events 
        public delegate void DeviceEventHandler(Object sender, DeviceChangedEventArgs e);

        #endregion

        #region Constructor

        private DeviceDetector()
        {
              _isWindowConfiged = false;
        }
        #endregion



        #region Method

        public static DeviceDetector GetInstance(Window window = null)
        {
            if (_deviceDetector == null)
                _deviceDetector = new DeviceDetector();
            return _deviceDetector;
        }

        private void HookWndProc(Window window)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        public void ConfigTargetWindow(Window window)
        {
            _isWindowConfiged = true;
            _windowKeeper = new WindowKeeper(window);
            HookWndProc(window);
            RegisterForDeviceChange();
        }

        /// <summary>
        /// Registers to be notified when devices are added or removed.
        /// </summary>
        /// <param name="register">True to register, False to unregister</param>
        /// <returns>True if successfull, False otherwise</returns>
        public bool RegisterForDeviceChange()
        {
            if (!_isWindowConfiged)
                throw new Exception(@"Please call ""ConfigTargetWindow"" before use!!");

            var handle = _windowKeeper.Handle;
            var deviceInterface = new Win32Wrapper.CTypes.DEV_BROADCAST_DEVICEINTERFACE();
            int size = Marshal.SizeOf(deviceInterface);
            IntPtr buffer = IntPtr.Zero;
            bool status = false;
            try
            {
                deviceInterface.dbch_size = size;
                deviceInterface.dbch_devicetype = (Int32) Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE;
                buffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(deviceInterface, buffer, true);
                _interfaceNotificationHandle = new SafeDeviceHandle(Win32Wrapper.CDevice.RegisterDeviceNotification(handle, buffer,
                                                                                         (int)
                                                                                         (Win32Wrapper.CTypes.DEVICE_NOTIFY
                                                                                                      .DEVICE_NOTIFY_WINDOW_HANDLE |
                                                                                          Win32Wrapper.CTypes.DEVICE_NOTIFY
                                                                                                      .DEVICE_NOTIFY_ALL_INTERFACE_CLASSES)));
                if (_interfaceNotificationHandle != null && !_interfaceNotificationHandle.IsInvalid)
                    status =  true;
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message,ex.ErrorCode.ToString());
                Logger.DebugFormat("Error Code[{0}] : [{1}]", ex.ErrorCode, ex.Message);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
            return status;
        }

        private static int _count = 0;

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (!_isWindowConfiged)
                throw new Exception(@"Please call ""ConfigTargetWindow"" before use!!");
            switch (msg)
            {
                case (int)Win32Wrapper.WindowsMessage.WM_DEVICECHANGE:
                    OnDeviceChanged(wParam, lParam);
                    break;
            }
            return (System.IntPtr)0;
        }

        private void OnDeviceChanged(IntPtr wParam, IntPtr lParam)
        {
            Logger.DebugFormat("++++++++++++++++++++++++++++++++++++++++++++{0}++++++++++++++++++++++++++++++++++++++++++++",
                               ++_count);
            // WM_DEVICECHANGE can have several meanings depending on the WParam value...
            Win32Wrapper.CTypes.DBTDEVICE dbtdevice = (Win32Wrapper.CTypes.DBTDEVICE) wParam.ToInt32();
            Win32Wrapper.CTypes.DBTDEVTYP devType = (Win32Wrapper.CTypes.DBTDEVTYP) Marshal.ReadInt32(lParam, 4);
            Logger.DebugFormat("DBTDEVICE: [{0}], [hex: {0:x}], [dec: {0:d}]", dbtdevice);
            Logger.DebugFormat("DBTDEVTYP: [{0}], [hex: {0:x}], [dec: {0:d}]", devType);
            switch (dbtdevice)
            {
                case Win32Wrapper.CTypes.DBTDEVICE.DBT_DEVICEARRIVAL:
                    // New device has just arrived
                    DeviceAttached(this, new DeviceChangedEventArgs(lParam));
                    break;

                case Win32Wrapper.CTypes.DBTDEVICE.DBT_DEVICEREMOVECOMPLETE:
                    DeviceRemoved(this, new DeviceChangedEventArgs(lParam));
                    break;
            }
            Logger.DebugFormat("++++++++++++++++++++++++++++++++++++++++++++{0}++++++++++++++++++++++++++++++++++++++++++++",
                               _count);
        }

        #endregion
        //public void ProcessWindowsMessage(ref Message m)
        //{
        //    Win32Wrapper.CTypes.DBTDEVTYP devType;

        //    if (m.Msg == (decimal) Win32Wrapper.WindowsMessage.WM_DEVICECHANGE)
        //    {
        //        // WM_DEVICECHANGE can have several meanings depending on the WParam value...
        //        switch (m.WParam.ToInt32())
        //        {
        //            case (int)Win32Wrapper.CTypes.DBTDEVICE.DBT_DEVICEARRIVAL:
        //                // New device has just arrived
        //                devType = (Win32Wrapper.CTypes.DBTDEVTYP)Marshal.ReadInt32(m.LParam, 4);
        //                if (devType == Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE)
        //                {
        //                    var e = new DeviceChangedEventArgs();
        //                    DeviceAttached(this, e);
        //                }
        //                break;

        //            case (int)Win32Wrapper.CTypes.DBTDEVICE.DBT_DEVICEQUERYREMOVE:
        //                // Device is about to be removed, any application can cancel the removal
        //                devType = (Win32Wrapper.CTypes.DBTDEVTYP)Marshal.ReadInt32(m.LParam, 4);
        //                if (devType == Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE)
        //                {
        //                    var e = new DeviceChangedEventArgs();
        //                    /*Win32Wrapper.DEV_BROADCAST_DEVICEINTERFACE deviceInterface = new Win32Wrapper.DEV_BROADCAST_DEVICEINTERFACE();

        //                    int size = Marshal.SizeOf(deviceInterface);
        //                    deviceInterface.dbcc_size = size;
        //                    Marshal.PtrToStructure(m.LParam, typeof(Win32Wrapper.DEV_BROADCAST_DEVICEINTERFACE));*/

        //                    DeviceQueryRemove(this, e);
        //                }
        //                break;
        //            case (int)Win32Wrapper.CTypes.DBTDEVICE.DBT_DEVICEREMOVECOMPLETE:
        //                // Device has been removed
        //                devType = (Win32Wrapper.CTypes.DBTDEVTYP)Marshal.ReadInt32(m.LParam, 4);
        //                if (devType == Win32Wrapper.CTypes.DBTDEVTYP.DBT_DEVTYP_DEVICEINTERFACE)
        //                {
        //                    DeviceChangedEventArgs e = new DeviceChangedEventArgs();
        //                    DeviceRemoved(this, e);
        //                }
        //                break;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Enumerate all USB devices and look for the device whose VID and PID are provided.
        ///// </summary>
        ///// <param name="vid">The Vendor ID of the USB Device.</param>
        ///// <param name="pid">The Product ID of the USB Device.</param>
        ///// <param name="deviceProperties">A Device Properties structure.</param>
        ///// <param name="getComPort">Set to True to retrieve the COM Port number if the Device is emulating a Serial Port.</param>
        ///// <returns>True the Device is found.</returns>
        //public static bool GetUSBDevice(UInt32 vid, UInt32 pid, ref Win32Wrapper.CDevice.DeviceProperties deviceProperties, bool getComPort)
        //{
        //    IntPtr intPtrBuffer = Marshal.AllocHGlobal(BufferSize);
        //    IntPtr h = IntPtr.Zero;
        //    bool status = false;

        //    try
        //    {
        //        const string devEnum = "USB";
        //        string expectedDeviceId = "VID_" + vid.ToString("X4") + "&" + "PID_" + pid.ToString("X4");
        //        expectedDeviceId = expectedDeviceId.ToLowerInvariant();

        //        h = Win32Wrapper.CDevice.SetupDiGetClassDevs(IntPtr.Zero, devEnum, IntPtr.Zero, (int)(Win32Wrapper.CTypes.DIGCF.DIGCF_PRESENT | Win32Wrapper.CTypes.DIGCF.DIGCF_ALLCLASSES));
        //        if (h.ToInt32() != InvalidHandleValue)
        //        {
        //            bool success = true;
        //            uint i = 0;
        //            while (success)
        //            {
        //                Win32Wrapper.Error lastError;
        //                if (success)
        //                {
        //                    UInt32 requiredSize = 0;
        //                    UInt32 regType = 0;
        //                    IntPtr ptr = IntPtr.Zero;

        //                    //Create a Device Info Data structure
        //                    var devInfoData = new Win32Wrapper.CTypes.SP_DEVINFO_DATA();
        //                    devInfoData.cbSize = (uint)Marshal.SizeOf(devInfoData);
        //                    success = Win32Wrapper.CDevice.SetupDiEnumDeviceInfo(h, i, ref devInfoData);

        //                    if (success)
        //                    {
        //                        //Get the required buffer size
        //                        //First query for the size of the hardware ID, so we can know how big a buffer to allocate for the data.
        //                        Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_HARDWAREID, ref regType, IntPtr.Zero, 0, ref requiredSize);

        //                        lastError = (Win32Wrapper.Error)Marshal.GetLastWin32Error();
        //                        if (lastError == Win32Wrapper.Error.ERROR_INSUFFICIENT_BUFFER)
        //                        {
        //                            if (requiredSize > BufferSize)
        //                            {
        //                                status = false;
        //                            }
        //                            else
        //                            {
        //                                if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_HARDWAREID, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                {
        //                                    string hardwareId = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                    hardwareId = hardwareId.ToLowerInvariant();
        //                                    if (hardwareId.Contains(expectedDeviceId))
        //                                    {
        //                                        status = true; //Found device
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_FRIENDLYNAME, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.FriendlyName = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_DEVTYPE, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DeviceType = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_CLASS, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DeviceClass = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_MFG, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DeviceManufacturer = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_LOCATION_INFORMATION, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DeviceLocation = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_LOCATION_PATHS, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DevicePath = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_PHYSICAL_DEVICE_OBJECT_NAME, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DevicePhysicalObjectName = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }
        //                                        if (Win32Wrapper.CDevice.SetupDiGetDeviceRegistryProperty(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.SPDRP.SPDRP_DEVICEDESC, ref regType, intPtrBuffer, BufferSize, ref requiredSize))
        //                                        {
        //                                            deviceProperties.DeviceDescription = Marshal.PtrToStringAuto(intPtrBuffer);
        //                                        }

        //                                        if (getComPort)
        //                                        {
        //                                            //Open the Device Parameters Key of the device

        //                                            IntPtr hDeviceRegistryKey = Win32Wrapper.CDevice.SetupDiOpenDevRegKey(h, ref devInfoData, (UInt32)Win32Wrapper.CTypes.DICS_FLAG.DICS_FLAG_GLOBAL, 0, (UInt32)Win32Wrapper.CTypes.DIREG.DIREG_DEV, (UInt32)Win32Wrapper.CTypes.REGKEYSECURITY.KEY_READ);
        //                                            if (hDeviceRegistryKey.ToInt32() == InvalidHandleValue)
        //                                            {
        //                                                lastError = (Win32Wrapper.Error)Marshal.GetLastWin32Error();
        //                                                break; //throw new Exception("Error opening the Registry: " + LastError.ToString());
        //                                            }
        //                                            else
        //                                            {
        //                                                UInt32 type = 0;
        //                                                var data = new System.Text.StringBuilder(BufferSize);
        //                                                UInt32 size = (UInt32)data.Capacity;

        //                                                int result = Win32Wrapper.CRegistry.RegQueryValueEx(hDeviceRegistryKey, "PortName", 0, out type, data, ref size);
        //                                                if (result == (int)Win32Wrapper.Error.ERROR_SUCCESS)
        //                                                {
        //                                                    deviceProperties.COMPort = data.ToString();
        //                                                }

        //                                                //Close Registry
        //                                                Win32Wrapper.CRegistry.RegCloseKey(hDeviceRegistryKey);
        //                                            }
        //                                        }
        //                                        break;
        //                                    }
        //                                    else
        //                                    {
        //                                        status = false;
        //                                    } //End of if (HardwareID.Contains(ExpectedDeviceID))
        //                                }
        //                            } //End of if (RequiredSize > BUFFER_SIZE)
        //                        } //End of if (LastError == Win32Wrapper.WinErrors.ERROR_INSUFFICIENT_BUFFER)
        //                    } // End of if (Success)
        //                } // End of if (Success)
        //                else
        //                {
        //                    lastError = (Win32Wrapper.Error)Marshal.GetLastWin32Error();
        //                    status = false;
        //                }
        //                i++;
        //            } // End of while (Success)
        //        } //End of if (h.ToInt32() != INVALID_HANDLE_VALUE)

        //        return status;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Win32Wrapper.CDevice.SetupDiDestroyDeviceInfoList(h); //Clean up the old structure we no longer need.
        //        Marshal.FreeHGlobal(intPtrBuffer);
        //    }
        //}


    }
}
