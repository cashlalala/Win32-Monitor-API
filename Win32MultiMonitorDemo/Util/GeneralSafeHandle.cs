using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Win32MultiMonitorDemo.Util
{
    public class SafeDeviceHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeDeviceHandle()
            : base(true)
        {
        }

        public SafeDeviceHandle(IntPtr pHandle)
            : base(true)
        {
            SetHandle(pHandle);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                bool bSuccess = Win32Wrapper.CDevice.UnregisterDeviceNotification(handle);
                handle = IntPtr.Zero;
                return bSuccess;
            }
            return false;
        }

        public static implicit operator IntPtr(SafeDeviceHandle generalSafeHandle)
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

        public static implicit operator HandleRef(SafeDeviceHandle generalSafeHandle)
        {
            return new HandleRef(generalSafeHandle, generalSafeHandle.handle);
        }

    }
}
