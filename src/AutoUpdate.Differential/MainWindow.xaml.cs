using GeneralUpdate.Differential;
using GeneralUpdate.Differential.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoUpdate.Differential
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var path1 = @"D:\TestCode\compare\source";
                var path2 = @"D:\TestCode\compare\target";
                var path3 = @"D:\TestCode\compare\patchs";
                await DifferentialCore.Instance.Clean(path1, path2, path3);
            });
        }

        private void BtnDrity_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var path1 = @"D:\TestCode\compare\source";
                //var path2 = @"D:\TestCode\compare\target";
                var path3 = @"D:\TestCode\compare\patchs";
                await DifferentialCore.Instance.Drity(path1, path3);
            });
        }

        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var path1 = @"D:\TestCode\compare\source";
                var path2 = @"D:\TestCode\compare\target";
                await ConfigFactory.Instance.Scan(path1, path2);
            });
        }

        private void BtnDeploy_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await ConfigFactory.Instance.Deploy();
            });
        }
    }
}
