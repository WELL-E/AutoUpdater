using AutoUpdate.WpfApp.ViewModels;
using GeneralUpdate.Core.Utils;
using GeneralUpdate.WpfApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoUpdate.WpfApp
{
    class Program : Application, ISingleInstanceApp
    {
        private const string AppId = "{7F280539-0814-4F9C-95BF-D2BB60023657}";

        [STAThread]
        static void Main(string[] args)
        {
            string[] resultArgs = null;

            if (args == null || args.Length == 0)
            {
                resultArgs = new string[6] {
                "0.0.0.0",
                "1.1.1.1",
                "https://github.com/WELL-E",
                 "http://192.168.50.225:7000/update.zip",
                 @"E:\PlatformPath",
                "371ada8087cf7651eafaadf4fcc214a5",
                 };
            }
            else
            {
                resultArgs = args;
            }

            if (resultArgs.Length != 6) return;
            if (SingleInstance<Program>.InitializeAsFirstInstance(AppId))
            {
                var win = new MainWindow();
                var vm = new MainViewModel(resultArgs, win.Close);
                win.DataContext = vm;

                var application = new Program();
                application.Run(win);
                SingleInstance<Program>.Cleanup();
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
