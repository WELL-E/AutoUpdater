using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace AutoUpdate.MauiApp_Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
#if WINDOWS
            var args = activationState.LaunchActivatedEventArgs.Arguments;
            MainPage = new MainPage(args);
#endif
            return base.CreateWindow(activationState);
        }
    }
}
