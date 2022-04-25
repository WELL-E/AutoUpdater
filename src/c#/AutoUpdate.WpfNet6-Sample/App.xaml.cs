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
            //var args = e.Args[0];
            var args = "eyJBcHBUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5DbGllbnRDb3JlIiwiTWFpbkFwcE5hbWUiOm51bGwsIkluc3RhbGxQYXRoIjoiRDpcXGdpdF9jb21tdW5pdHlcXEdlbmVyYWxVcGRhdGVcXHNyY1xcYyNcXEF1dG9VcGRhdGUuQ2xpZW50Q29yZVxcYmluXFxEZWJ1Z1xcbmV0Ni4wLXdpbmRvd3MiLCJDbGllbnRWZXJzaW9uIjoiMS4wLjAuMCIsIkxhc3RWZXJzaW9uIjoiOS4xLjMuMCIsIlVwZGF0ZUxvZ1VybCI6bnVsbCwiSXNVcGRhdGUiOmZhbHNlLCJVcGRhdGVVcmwiOm51bGwsIlZhbGlkYXRlVXJsIjpudWxsLCJNYWluVXBkYXRlVXJsIjoiaHR0cDovLzEyNy4wLjAuMTo1MDAxL3ZlcnNpb25zLzEvMi40LjcuMCIsIk1haW5WYWxpZGF0ZVVybCI6Imh0dHA6Ly8xMjcuMC4wLjE6NTAwMS92YWxpZGF0ZS8xLzIuNC43LjAiLCJDb21wcmVzc0VuY29kaW5nIjo3LCJDb21wcmVzc0Zvcm1hdCI6Ii56aXAiLCJEb3dubG9hZFRpbWVPdXQiOjYwLCJVcGRhdGVWZXJzaW9ucyI6W3siUHViVGltZSI6MTYyNjcxMTc2MCwiTmFtZSI6bnVsbCwiTUQ1IjoiMWJmZDcyMzYyNThiMTJjNTFmZDA5ZjEzODA4MjM1ZGYiLCJWZXJzaW9uIjoiOS4xLjMuMCIsIlVybCI6bnVsbH1dfQ==";
            MainWindow window = new MainWindow(args);
            window.ShowDialog();
            base.OnStartup(e);
        }
    }
}