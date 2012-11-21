using System;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Win32MultiMonitorDemo.Util
{
    public static class Win32
    {
        public class GeneralSafeHandle : SafeHandle
        {
            public GeneralSafeHandle(IntPtr hHandle)
                : base(IntPtr.Zero,true)
            {
                SetHandle(hHandle);
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                // If ReleaseHandle failed, it can be reported via the
                // "releaseHandleFailed" managed debugging assistant (MDA).  This
                // MDA is disabled by default, but can be enabled in a debugger
                // or during testing to diagnose handle corruption problems.
                // We do not throw an exception because most code could not recover
                // from the problem.
                return CloseHandle(handle);
            }

            public static implicit operator IntPtr (GeneralSafeHandle generalSafeHandle)
            {
                return generalSafeHandle.handle;
            }

            //public static explicit operator IntPtr(GeneralSafeHandle generalSafeHandle)
            //{
            //    return generalSafeHandle.handle;
            //}

            //public static explicit operator HandleRef(GeneralSafeHandle generalSafeHandle)
            //{
            //    return new HandleRef(generalSafeHandle, generalSafeHandle.handle);
            //}

            public static implicit operator HandleRef(GeneralSafeHandle generalSafeHandle)
            {
                return new HandleRef(generalSafeHandle, generalSafeHandle.handle);
            }


            public override bool IsInvalid
            {
                get { return IsClosed || handle == IntPtr.Zero; }
            }
        }

        public sealed class SafeWindowStationHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public SafeWindowStationHandle()
                : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return CloseWindowStation(handle);
            }
        }

       [return: MarshalAs(UnmanagedType.Bool)]
       [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
       [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
       public static extern bool CloseWindowStation(IntPtr hWinsta);



        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool CloseHandle(IntPtr handle);


        public static class CMonitor
        {
#region Structure
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
            public class MONITORINFOEX
            {
                protected int _cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
                protected RECT _rcMonitor = new RECT();
                protected RECT _rcWork = new RECT();
                protected int _dwFlags = 0;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
                protected char[] _szDevice = new char[32];

                public virtual int CbSize
                {
                    get { return _cbSize; }
                    set { _cbSize = value; }
                }

                public virtual RECT rcMonitor
                {
                    get { return _rcMonitor; }
                    set { _rcMonitor = value; }
                }

                public virtual RECT rcWork
                {
                    get { return _rcWork; }
                    set { _rcWork = value; }
                }

                public virtual int dwFlags
                {
                    get { return _dwFlags; }
                    set { _dwFlags = value; }
                }

                public virtual char[] szDevice
                {
                    get { return _szDevice; }
                    set { _szDevice = value; }
                }

            }

            [StructLayout(LayoutKind.Sequential)]
            public struct POINTSTRUCT
            {
                public int x;
                public int y;
                public POINTSTRUCT(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
                public RECT(int l, int t, int r, int b)
                {
                    left = l;
                    top = t;
                    right = r;
                    bottom = b;
                }
                public override string ToString()
                {
                    return String.Format("left: {0}, top: {1}, right: {2}, buttom: {3}", left, top, right, bottom);
                }
            }
#endregion


            [DllImport("User32.dll", SetLastError = true,CharSet=CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] Win32MultiMonitorDemo.Util.Win32.CMonitor.MONITORINFOEX monitorInfo);

            [DllImport("User32.dll", SetLastError = true)]
            public static extern IntPtr MonitorFromPoint(Win32MultiMonitorDemo.Util.Win32.CMonitor.POINTSTRUCT pt, uint flags);


            /*
             * The window handle you found no need to be closed manually,
             * so here only use the IntPtr as the Input type.
             * HandleRef can also be used here.
             */
            [DllImport("User32.dll",SetLastError=true)]
            public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

#region Define MACRO
            public const uint MONITOR_DEFAULTTONULL = 0;
            public const uint MONITOR_DEFAULTTOPRIMARY = 1;
            public const uint MONITOR_DEFAULTTONEAREST = 2;
#endregion

            [DllImport("user32.dll", SetLastError=true)]
            public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, int dwData);

            public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, int dwData);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool EnumDesktops(IntPtr hwinsta, EnumDesktopsDelegate
                lpEnumFunc, IntPtr lParam);

            public delegate bool EnumDesktopsDelegate([MarshalAs(UnmanagedType.LPTStr)]string desktopName, int lParam);

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern SafeWindowStationHandle GetProcessWindowStation();
        
        }


    }
}
