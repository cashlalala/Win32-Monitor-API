using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using Win32MultiMonitorDemo.Util;
using Win32MultiMonitorDemo.ViewModels;
using log4net;

namespace Win32MultiMonitorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Field

        private ViewModel ViewModel { get; set; }

        //private HwndSource _hwndSource;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindow).Name);

        #endregion

#region Constructors
        public MainWindow()
        {
            InitializeComponent();

            Logger.Debug("MainWindow Init ");

            this.ViewModel = new ViewModels.ViewModel(this);
            this.DataContext = ViewModel;

        }
#endregion

        private void MainWin_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Logger.Debug("Handle Changed!!!");
            this.ViewModel.OnHandleUpdated(sender, e);
        }

        private void MainWin_SourceInitialized(object sender, EventArgs e)
        {
            Logger.Debug("Handle Initialized!!!");
            this.ViewModel.OnHandleUpdated(sender, e);
        }
      

    }


}
