using AutoUpdate.MauiApp_Sample.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using System;

namespace AutoUpdate.MauiApp_Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage() { InitializeComponent(); }

        public MainPage(string prameter)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(prameter);
            //wpf - DataContext
        }
    }
}
