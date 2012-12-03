using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using Win32MultiMonitorDemo.Util;

namespace Win32MultiMonitorDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var temp = new FileInfo("log4net.xml");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(temp);

        }
    }
}
