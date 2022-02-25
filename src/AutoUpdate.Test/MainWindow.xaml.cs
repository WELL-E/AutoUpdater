using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace AutoUpdate.Test
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

        #region Process launch

        private string InitBase64String(string jsonString)
        {
            var bytes = Encoding.Default.GetBytes(jsonString);
            var base64str = Convert.ToBase64String(bytes);
            return base64str;
        }

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string parameterString = TxtParmeterJson.Text;
                string base64String = InitBase64String(parameterString);
                Process.Start(TxtEXEPath.Text, base64String);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Parameter or base64 error : { ex.Message } ");
            }
        }

        #endregion

    }
}