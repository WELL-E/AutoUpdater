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
            //if (args.Length != 6) return; 
            if (SingleInstance<Program>.InitializeAsFirstInstance(AppId))
            {
                var win = new MainWindow();
                var vm = new MainViewModel(args, win.Close);
                win.DataContext = vm;

                var application = new Program();
                //application.InitializeComponent();
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
