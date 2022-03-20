using AutoUpdate.Core;
using System.Windows;

namespace AutoUpdate.WpfNet6_Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var args = e.Args;
            var args = "eyJBcHBUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5DbGllbnRDb3JlIiwiTWFpbkFwcE5hbWUiOm51bGwsIkluc3RhbGxQYXRoIjoiRDpcXFVwZGF0ZXRlc3RfaHViXFxSdW5fYXBwIiwiQ2xpZW50VmVyc2lvbiI6IjEuMS4xIiwiTGFzdFZlcnNpb24iOiI5LjEuNS4wIiwiVXBkYXRlTG9nVXJsIjpudWxsLCJJc1VwZGF0ZSI6ZmFsc2UsIlVwZGF0ZVVybCI6bnVsbCwiVmFsaWRhdGVVcmwiOm51bGwsIk1haW5VcGRhdGVVcmwiOiJodHRwOi8vMTI3LjAuMC4xOjUwMDEvdmVyc2lvbnMvMS8xLjEuMS4xIiwiTWFpblZhbGlkYXRlVXJsIjoiaHR0cDovLzEyNy4wLjAuMTo1MDAxL3ZhbGlkYXRlLzEvMS4xLjEuMSIsIkNvbXByZXNzRW5jb2RpbmciOjcsIkNvbXByZXNzRm9ybWF0IjoiLnppcCIsIkRvd25sb2FkVGltZU91dCI6NjAsIlVwZGF0ZVZlcnNpb25zIjpbeyJQdWJUaW1lIjoxNjI2NzExNzYwLCJOYW1lIjpudWxsLCJNRDUiOiI0MmU4MTVlMzI2NjE2ODQxZjA4ODUxYzc5NjdkZmRlMiIsIlZlcnNpb24iOiI5LjEuMy4wIiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX0seyJQdWJUaW1lIjoxNjI2NzExODIwLCJOYW1lIjpudWxsLCJNRDUiOiJkOWEzNzg1ZjA4ZWQzZGQ5Mjg3MmJkODA3ZWJmYjkxNyIsIlZlcnNpb24iOiI5LjEuNC4wIiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX0seyJQdWJUaW1lIjoxNjI2NzExODgwLCJOYW1lIjpudWxsLCJNRDUiOiIyMjRkYTU4NjU1M2Q2MDMxNWM1NWU2ODlhNzg5YjdiZCIsIlZlcnNpb24iOiI5LjEuNS4wIiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX1dfQ==";
            MainWindow window = new MainWindow(args);
            window.ShowDialog();
            base.OnStartup(e);
        }
    }
}