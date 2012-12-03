using System;
using System.Collections.Generic;

namespace Win32MultiMonitorDemo.Util
{
    public static class MonitorManagerFactory
    {
        public static IMonitorManager GetInstance(ManagerType managerType = ManagerType.Win32, Object parameter = null)
        {
            IMonitorManager manager = null;
            try
            {
                manager = MonitorManagerMap[managerType];
            }
            catch (Exception)
            {
                manager = (IMonitorManager)Activator.CreateInstance(ManagerTypeRecord[managerType], parameter);
                MonitorManagerMap.Add(managerType, manager);
            }
            return manager;
        }

        private static readonly Dictionary<ManagerType, IMonitorManager> MonitorManagerMap = new Dictionary<ManagerType, IMonitorManager>();

        private static readonly Dictionary<ManagerType, Type> ManagerTypeRecord = new Dictionary<ManagerType, Type>
            { 
            {ManagerType.Win32, typeof(Win32MonitorManager)},
            {ManagerType.Wpf, typeof(WpfMonitorManager)}
        };

        public enum ManagerType
        {
            Win32 = 1,
            Wpf = 2
        }

    }
}
