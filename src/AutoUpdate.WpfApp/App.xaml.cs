using AutoUpdate.WpfApp.ViewModels;
using GeneralUpdate.Core.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GeneralUpdate.WpfApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string AppId = "{7F280539-0814-4F9C-95BF-D2BB60023657}";

        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] resultArgs = null;

            if (e.Args == null || e.Args.Length == 0)
            {
                resultArgs = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.50.225:7000/update.zip",
                 @"E:\PlatformPath",
                "509f0ede227de4a662763a4abe3d8470",
                 };
            }
            else
            {
                resultArgs = e.Args;
            }

            if (resultArgs.Length != 6) return;
            if (SingleInstance<App>.InitializeAsFirstInstance(AppId))
            {
                var win = new MainWindow();
                var vm = new MainViewModel(resultArgs, win.Close);
                win.DataContext = vm;

                var application = new App();
                application.InitializeComponent();
                application.Run(win);
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = WindowState.Normal;
            }
            this.MainWindow.Activate();

            return true;
        }
    }
}
