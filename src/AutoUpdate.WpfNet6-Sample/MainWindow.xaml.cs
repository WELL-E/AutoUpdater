using AutoUpdate.WpfNet6_Sample.ViewModels;
using System.Windows;

namespace AutoUpdate.WpfNet6_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string args)
        {
            InitializeComponent();
            DataContext = new MainViewModel(args);
        }
    }
}