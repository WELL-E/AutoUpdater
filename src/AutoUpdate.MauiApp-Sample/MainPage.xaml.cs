using AutoUpdate.MauiApp_Sample.ViewModels;
using GeneralUpdate.Common.Models;
using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Windows;

namespace AutoUpdate.MauiApp_Sample;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
    }

	private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        CounterLabel.Text = $"Check for Update Current Count: {count}";

        SemanticScreenReader.Announce(CounterLabel.Text);

        var args = "eyJDbGllbnRUeXBlIjoxLCJBcHBOYW1lIjoiQXV0b1VwZGF0ZS5UZXN0IiwiTWFpbkFwcE5hbWUiOm51bGwsIkluc3RhbGxQYXRoIjoiRDpcXHVwZGF0ZV90ZXN0IiwiQ2xpZW50VmVyc2lvbiI6IjEuMS4xIiwiTGFzdFZlcnNpb24iOiIxLjEuNSIsIlVwZGF0ZUxvZ1VybCI6Imh0dHBzOi8vd3d3LmJhaWR1LmNvbS8iLCJJc1VwZGF0ZSI6ZmFsc2UsIlVwZGF0ZVVybCI6Imh0dHA6Ly8xMjcuMC4wLjE6NTAwMS92ZXJzaW9ucy8xLzEuMS4xIiwiVmFsaWRhdGVVcmwiOm51bGwsIk1haW5VcGRhdGVVcmwiOm51bGwsIk1haW5WYWxpZGF0ZVVybCI6bnVsbCwiVXBkYXRlVmVyc2lvbnMiOlt7IlB1YlRpbWUiOjE2MjY3MTE3NjAsIk5hbWUiOm51bGwsIk1ENSI6ImY2OThmOTAzMmMwZDU0MDFiYWNkM2IwZjUzMDk5NjE4IiwiVmVyc2lvbiI6IjEuMS4zIiwiVXJsIjpudWxsLCJJc1VuWmlwIjpmYWxzZX0seyJQdWJUaW1lIjoxNjI2NzExODIwLCJOYW1lIjpudWxsLCJNRDUiOiI2YTEwNDZhNjZjZWRmNTA5YmZiMmE3NzFiMmE3YTY0ZSIsIlZlcnNpb24iOiIxLjEuNCIsIlVybCI6bnVsbCwiSXNVblppcCI6ZmFsc2V9LHsiUHViVGltZSI6MTYyNjcxMTg4MCwiTmFtZSI6bnVsbCwiTUQ1IjoiNzY4OWM0NzJjZTczYTRiOGYxYjdjNzkxNzMxMzM3ZTEiLCJWZXJzaW9uIjoiMS4xLjUiLCJVcmwiOm51bGwsIklzVW5aaXAiOmZhbHNlfV19";
        var bootStrap = new GeneralUpdateBootstrap();
        bootStrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
        bootStrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
        bootStrap.MutiDownloadError += OnMutiDownloadError;
        bootStrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
        bootStrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
        bootStrap.Exception += OnException;
        bootStrap.Strategy<DefaultStrategy>().
        Option(UpdateOption.DownloadTimeOut, 60).
        Option(UpdateOption.Format, "zip").
        RemoteAddressBase64(args);
        bootStrap.LaunchAsync();
    }


    private void OnMutiDownloadStatistics(object sender, GeneralUpdate.Core.Update.MutiDownloadStatisticsEventArgs e)
    {
        Tips1.Text = $" { e.Speed } , { e.Remaining }";
    }

    private void OnMutiDownloadProgressChanged(object sender, GeneralUpdate.Core.Update.MutiDownloadProgressChangedEventArgs e)
    {
        switch (e.Type)
        {
            case ProgressType.Check:
                if (!string.IsNullOrEmpty(e.Message))
                {
                    Tips5.Text = e.Message;
                }
                break;
            case ProgressType.Donwload:
                //ProgressVal = e.BytesReceived;
                //if (ProgressMax != e.TotalBytesToReceive)
                //{
                //    ProgressMax = e.TotalBytesToReceive;
                //}
                Tips2.Text = $" { Math.Round(e.ProgressValue * 100, 2) }% ， Receivedbyte：{ e.BytesReceived }M ，Totalbyte：{ e.TotalBytesToReceive }M";
                break;
            case ProgressType.Updatefile:
                break;
            case ProgressType.Done:
                break;
            case ProgressType.Fail:
                break;
            default:
                break;
        }
    }

    private void OnMutiDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiDownloadCompletedEventArgs e)
    {
        //Tips3 = $"{ e.Version.Name } download completed.";
    }

    private void OnMutiAllDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiAllDownloadCompletedEventArgs e)
    {
        if (e.IsAllDownloadCompleted)
        {
            Tips4.Text = "AllDownloadCompleted";
        }
        else
        {
            //foreach (var version in e.FailedVersions)
            //{
            //    Debug.Write($"{ version.Item1.Name }");
            //}
        }
    }

    private void OnMutiDownloadError(object sender, GeneralUpdate.Core.Update.MutiDownloadErrorEventArgs e)
    {

        //Tips5 = $"{ e.Version.Name },{ e.Exception.Message }.";
    }

    private void OnException(object sender, GeneralUpdate.Core.Update.ExceptionEventArgs e)
    {
        Tips6.Text = $"{ e.Exception.Message }";
    }
}

