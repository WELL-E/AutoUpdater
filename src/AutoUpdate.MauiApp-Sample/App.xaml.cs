﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoUpdate.MauiApp_Sample; 

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainPage();

    }
}
