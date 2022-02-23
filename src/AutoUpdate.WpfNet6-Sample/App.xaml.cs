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
            e.Args[0] = "eyJDbGllbnRUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5UZXN0IiwiTWFpbkFwcE5hbWUiOm51bGwsIkluc3RhbGxQYXRoIjoiRDpcXHVwZGF0ZV90ZXN0IiwiQ2xpZW50VmVyc2lvbiI6IjEuMS4xIiwiTGFzdFZlcnNpb24iOiIxLjEuNSIsIlVwZGF0ZUxvZ1VybCI6Imh0dHBzOi8vd3d3LmJhaWR1LmNvbS8iLCJJc1VwZGF0ZSI6ZmFsc2UsIlVwZGF0ZVVybCI6Imh0dHA6Ly8xMjcuMC4wLjE6NTAwMS92ZXJzaW9ucy8xLzEuMS4xIiwiVmFsaWRhdGVVcmwiOm51bGwsIk1haW5VcGRhdGVVcmwiOm51bGwsIk1haW5WYWxpZGF0ZVVybCI6bnVsbCwiVXBkYXRlVmVyc2lvbnMiOlt7IlB1YlRpbWUiOjE2MjY3MTE3NjAsIk5hbWUiOm51bGwsIk1ENSI6ImY2OThmOTAzMmMwZDU0MDFiYWNkM2IwZjUzMDk5NjE4IiwiVmVyc2lvbiI6IjEuMS4zIiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX0seyJQdWJUaW1lIjoxNjI2NzExODIwLCJOYW1lIjpudWxsLCJNRDUiOiI2YTEwNDZhNjZjZWRmNTA5YmZiMmE3NzFiMmE3YTY0ZSIsIlZlcnNpb24iOiIxLjEuNCIsIlVybCI6bnVsbCwiSXNVblppcCI6ZmFsc2V9LHsiUHViVGltZSI6MTYyNjcxMTg4MCwiTmFtZSI6bnVsbCwiTUQ1IjoiNzY4OWM0NzJjZTczYTRiOGYxYjdjNzkxNzMxMzM3ZTEiLCJWZXJzaW9uIjoiMS4xLjUiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfV19";
            var win = new MainWindow(e.Args[0]);
            win.ShowDialog();
            base.OnStartup(e);
        }
    }
}