using Avalonia.Controls;

namespace GeneralUpdate.PacketTool.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}
