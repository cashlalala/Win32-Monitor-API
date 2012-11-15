using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace Win32MultiMonitorDemo.Util
{
    public static class Win32
    {
        public class GeneralSafeHandle : SafeHandle
        {

            private GeneralSafeHandle()
                : base(IntPtr.Zero, //invalid value
                         true)           //is released at the end of SafeHandle
            {
            }

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
                return Win32.CloseHandle(this.handle);
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

        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool CloseHandle(IntPtr handle);


        public static class MultiMonitorHelper
        {
            [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto, Pack=4)]
            public class MONITORINFOEX { 
                public int     cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
                public RECT    rcMonitor = new RECT(); 
                public RECT    rcWork = new RECT(); 
                public int     dwFlags = 0;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
                public char[]  szDevice = new char[32];
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct POINTSTRUCT { 
                public int x;
                public int y;
                public POINTSTRUCT(int x, int y) {
                  this.x = x; 
                  this.y = y;
                } 
            } 

            [StructLayout(LayoutKind.Sequential)] 
            public struct RECT {
                public int left; 
                public int top; 
                public int right;
                public int bottom; 
            }

            [DllImport("User32.dll", SetLastError = true,CharSet=CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetMonitorInfo(HandleRef hMonitor, [In, Out] Win32MultiMonitorDemo.Util.Win32.MultiMonitorHelper.MONITORINFOEX monitorInfo);

            [DllImport("User32.dll", SetLastError = true)]
            public static extern GeneralSafeHandle MonitorFromPoint(Win32MultiMonitorDemo.Util.Win32.MultiMonitorHelper.POINTSTRUCT pt, uint flags);


            /*
             * The window handle you found no need to be closed manually,
             * so here only use the IntPtr as the Input type.
             * HandleRef can also be used here.
             */
            [DllImport("User32.dll",SetLastError=true)]
            public static extern GeneralSafeHandle MonitorFromWindow(IntPtr hWnd, uint dwFlags);


            public const uint MONITOR_DEFAULTTONULL = 0;
            public const uint MONITOR_DEFAULTTOPRIMARY = 1;
            public const uint MONITOR_DEFAULTTONEAREST = 2;
        
        }


    }
}
