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
            string args = @"eyJDbGllbnRUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5NYXVpQXBwLVNhbXBsZSIsIk1haW5BcHBOYW1lIjpudWxsLCJJbnN0YWxsUGF0aCI6IkQ6XFx1cGRhdGVfdGVzdCIsIkNsaWVudFZlcnNpb24iOiIxLjEuMSIsIkxhc3RWZXJzaW9uIjoiMS4xLjUiLCJVcGRhdGVMb2dVcmwiOiJodHRwczovL3d3dy5iYWlkdS5jb20vIiwiSXNVcGRhdGUiOmZhbHNlLCJVcGRhdGVVcmwiOiJodHRwOi8vMTI3LjAuMC4xOjUwMDEvdmVyc2lvbnMvMS8xLjEuMSIsIlZhbGlkYXRlVXJsIjpudWxsLCJNYWluVXBkYXRlVXJsIjpudWxsLCJNYWluVmFsaWRhdGVVcmwiOm51bGwsIlVwZGF0ZVZlcnNpb25zIjpbeyJQdWJUaW1lIjoxNjI2NzExNzYwLCJOYW1lIjpudWxsLCJNRDUiOiJmNjk4ZjkwMzJjMGQ1NDAxYmFjZDNiMGY1MzA5OTYxOCIsIlZlcnNpb24iOiIxLjEuMyIsIlVybCI6bnVsbCwiSXNVblppcCI6ZmFsc2V9LHsiUHViVGltZSI6MTYyNjcxMTgyMCwiTmFtZSI6bnVsbCwiTUQ1IjoiNmExMDQ2YTY2Y2VkZjUwOWJmYjJhNzcxYjJhN2E2NGUiLCJWZXJzaW9uIjoiMS4xLjQiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfSx7IlB1YlRpbWUiOjE2MjY3MTE4ODAsIk5hbWUiOm51bGwsIk1ENSI6Ijc2ODljNDcyY2U3M2E0YjhmMWI3Yzc5MTczMTMzN2UxIiwiVmVyc2lvbiI6IjEuMS41IiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX1dfQ==";

#if WINDOWS
            //var args = activationState.LaunchActivatedEventArgs.Arguments;
            MainPage = new MainPage(args);
#endif
            return base.CreateWindow(activationState);
        }
    }
}
