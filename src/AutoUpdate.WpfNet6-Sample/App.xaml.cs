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
            //e.Args[0] = "eyJBcHBUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5DbGllbnRDb3JlIiwiTWFpbkFwcE5hbWUiOm51bGwsIkluc3RhbGxQYXRoIjoiRDpcXGdpdF9jb21tdW5pdHlcXEdpdEh1YlxcR2VuZXJhbFVwZGF0ZVxcc3JjXFxBdXRvVXBkYXRlLkNsaWVudENvcmVcXGJpblxcRGVidWdcXG5ldDYuMC13aW5kb3dzIiwiQ2xpZW50VmVyc2lvbiI6IjEuMC4wLjAiLCJMYXN0VmVyc2lvbiI6IjkuMS41LjAiLCJVcGRhdGVMb2dVcmwiOm51bGwsIklzVXBkYXRlIjpmYWxzZSwiVXBkYXRlVXJsIjpudWxsLCJWYWxpZGF0ZVVybCI6bnVsbCwiTWFpblVwZGF0ZVVybCI6Imh0dHA6Ly8xMjcuMC4wLjE6NTAwMS92ZXJzaW9ucy8xLzEuMS4yLjAiLCJNYWluVmFsaWRhdGVVcmwiOiJodHRwOi8vMTI3LjAuMC4xOjUwMDEvdmFsaWRhdGUvMS8xLjEuMi4wIiwiQ29tcHJlc3NFbmNvZGluZyI6NywiQ29tcHJlc3NGb3JtYXQiOiIuemlwIiwiRG93bmxvYWRUaW1lT3V0Ijo2MCwiVXBkYXRlVmVyc2lvbnMiOlt7IlB1YlRpbWUiOjE2MjY3MTE3NjAsIk5hbWUiOm51bGwsIk1ENSI6IjQyZTgxNWUzMjY2MTY4NDFmMDg4NTFjNzk2N2RmZGUyIiwiVmVyc2lvbiI6IjkuMS4zLjAiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfSx7IlB1YlRpbWUiOjE2MjY3MTE4MjAsIk5hbWUiOm51bGwsIk1ENSI6ImQ5YTM3ODVmMDhlZDNkZDkyODcyYmQ4MDdlYmZiOTE3IiwiVmVyc2lvbiI6IjkuMS40LjAiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfSx7IlB1YlRpbWUiOjE2MjY3MTE4ODAsIk5hbWUiOm51bGwsIk1ENSI6IjIyNGRhNTg2NTUzZDYwMzE1YzU1ZTY4OWE3ODliN2JkIiwiVmVyc2lvbiI6IjkuMS41LjAiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfV19";
            var win = new MainWindow(e.Args[0]);
            win.ShowDialog();
            base.OnStartup(e);
        }
    }
}