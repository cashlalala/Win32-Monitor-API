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

using Win32MultiMonitorDemo.ViewModels;

namespace Win32MultiMonitorDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
#region Field
        private ViewModels.ViewModel viewModel;

        public ViewModels.ViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }
#endregion

#region Constructors
        public MainWindow()
        {
            InitializeComponent();

            this.viewModel = new ViewModels.ViewModel(this);
            this.DataContext = ViewModel;
        }
#endregion

    }


}
