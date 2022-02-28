namespace AutoUpdate.MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            this.DisplayAlert("title","hello,world.","ok");
        }
    }
}