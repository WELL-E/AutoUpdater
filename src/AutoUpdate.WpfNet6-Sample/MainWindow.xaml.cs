using AutoUpdate.Core.ViewModels;
using System.Windows;

namespace AutoUpdate.Core
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