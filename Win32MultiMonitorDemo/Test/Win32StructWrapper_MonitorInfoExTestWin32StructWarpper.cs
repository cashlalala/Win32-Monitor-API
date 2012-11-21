using Win32MultiMonitorDemo.Util;
using Win32MultiMonitorDemo.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test
{
    
    
    /// <summary>
    ///This is a test class for Win32StructWrapper_MonitorInfoExTestWin32StructWarpper and is intended
    ///to contain all Win32StructWrapper_MonitorInfoExTestWin32StructWarpper Unit Tests
    ///</summary>
    [TestClass()]
    public class Win32StructWrapper_MonitorInfoExTestWin32StructWarpper
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        ///// <summary>
        /////A test for MonitorInfoEx Constructor
        /////</summary>
        //[TestMethod()]
        //public void Win32StructWrapper_MonitorInfoExConstructorTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for OnPropertyChanged
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("Win32MultiMonitorDemo.exe")]
        //public void OnPropertyChangedTest()
        //{
        //    Win32StructWrapper_Accessor.MonitorInfoEx target = new Win32StructWrapper_Accessor.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    string property = string.Empty; // TODO: Initialize to an appropriate value
        //    target.OnPropertyChanged(property);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        /// <summary>
        ///A test for copy2Win32Struct
        ///</summary>
        [TestMethod()]
        public void copy2Win32StructTest()
        {
            Win32MultiMonitorDemo.Model.Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
            Win32StructWrapper.MonitorInfoEx monitorInfoEx = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
            monitorInfoEx.dwFlags = 1;
            monitorInfoEx.rcMonitor = new Win32.MultiMonitor.RECT();
            monitorInfoEx.rcWork = new Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT();
            monitorInfoEx.szDevice = new char[] { 'm','o','n','i','t','o','r','1' };
            Win32.MultiMonitor.MONITORINFOEX expected = new Win32.MultiMonitor.MONITORINFOEX(); // TODO: Initialize to an appropriate value
            expected.dwFlags = 1;
            expected.rcMonitor = new Win32MultiMonitorDemo.Util.Win32.MultiMonitor.RECT();
            expected.rcWork = new Win32.MultiMonitor.RECT();
            expected.szDevice = new char[] { 'm', 'o', 'n', 'i', 't', 'o', 'r', '1' };
            Win32.MultiMonitor.MONITORINFOEX actual;
            actual = target.copy2Win32Struct(monitorInfoEx);
            Assert.AreEqual(expected, actual);
            
        }

        ///// <summary>
        /////A test for cbSize
        /////</summary>
        //[TestMethod()]
        //public void cbSizeTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.cbSize = expected;
        //    actual = target.cbSize;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for dwFlags
        /////</summary>
        //[TestMethod()]
        //public void dwFlagsTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.dwFlags = expected;
        //    actual = target.dwFlags;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for rcMonitor
        /////</summary>
        //[TestMethod()]
        //public void rcMonitorTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    Win32.MultiMonitor.RECT expected = new Win32.MultiMonitor.RECT(); // TODO: Initialize to an appropriate value
        //    Win32.MultiMonitor.RECT actual;
        //    target.rcMonitor = expected;
        //    actual = target.rcMonitor;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for rcWork
        /////</summary>
        //[TestMethod()]
        //public void rcWorkTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    Win32.MultiMonitor.RECT expected = new Win32.MultiMonitor.RECT(); // TODO: Initialize to an appropriate value
        //    Win32.MultiMonitor.RECT actual;
        //    target.rcWork = expected;
        //    actual = target.rcWork;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for szDevice
        /////</summary>
        //[TestMethod()]
        //public void szDeviceTest()
        //{
        //    Win32StructWrapper.MonitorInfoEx target = new Win32StructWrapper.MonitorInfoEx(); // TODO: Initialize to an appropriate value
        //    char[] expected = null; // TODO: Initialize to an appropriate value
        //    char[] actual;
        //    target.szDevice = expected;
        //    actual = target.szDevice;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}

