using GeneralUpdate.Zip;
using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace AutoUpdate.ZIP
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

        #region GeneralUpdate Zip

        /// <summary>
        /// Create Zip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateZip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var factory = new GeneralZipFactory();
                factory.CompressProgress += OnCompressProgress;
                //压缩该路径下所有的文件：D:\Updatetest_hub\Run_app ， D:\Updatetest_hub
                factory.CreatefOperate(GetOperationType(), TxtZipPath.Text, TxtUnZipPath.Text).
                    CreatZip();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CreatZip error : { ex.Message } ");
            }
        }

        /// <summary>
        /// UnZip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUnZip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var factory = new GeneralZipFactory();
                factory.UnZipProgress += OnUnZipProgress;
                factory.Completed += OnCompleted;
                //解压文件包：D:\Updatetest_hub\Run_app\1.zip , D:\Updatetest_hub
                factory.CreatefOperate(GetOperationType(), TxtZipPath.Text, TxtUnZipPath.Text, true).
                    UnZip();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UnZip error : { ex.Message } ");
            }
        }

        private void OnCompleted(object sender, CompleteEventArgs e)
        {
            Debug.WriteLine($"IsCompleted { e.IsCompleted }.");
        }

        private OperationType GetOperationType()
        {
            OperationType operationType = 0;
            var item = CmbxZipFormat.SelectedItem as ComboBoxItem;
            switch (item.Content)
            {
                case "ZIP":
                    operationType = OperationType.GZip;
                    break;
                case "7z":
                    operationType = OperationType.G7z;
                    break;
            }
            return operationType;
        }

        private void OnCompressProgress(object sender, BaseCompressProgressEventArgs e) { Debug.WriteLine($"CompressProgress - name:{ e.Name }, count:{ e.Count }, index:{ e.Index }, size:{ e.Size }."); }

        private void OnUnZipProgress(object sender, BaseUnZipProgressEventArgs e) { Debug.WriteLine($"UnZipProgress - name:{ e.Name }, count:{ e.Count }, index:{ e.Index }, size:{ e.Size }."); }

        #endregion
    }
}
