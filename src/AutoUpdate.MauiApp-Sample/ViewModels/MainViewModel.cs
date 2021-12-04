using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoUpdate.MauiApp_Sample.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private string _resultBase64;
        private string _tips1, _tips2, _tips3, _tips4 , _tips5, _tips6;

        public ICommand LaunchCommand => new AsyncCommand<object>(LaunchCommandExecute);

        public string Tips1 { get => _tips1; set => SetProperty(ref _tips1 , value); }
        public string Tips2 { get => _tips2; set => SetProperty(ref _tips2, value); }
        public string Tips3 { get => _tips3; set => SetProperty(ref _tips3, value); }
        public string Tips4 { get => _tips4; set => SetProperty(ref _tips4, value); }
        public string Tips5 { get => _tips5; set => SetProperty(ref _tips5, value); }
        public string Tips6 { get => _tips6; set => SetProperty(ref _tips6, value); }

        internal MainViewModel(string prameter) 
        {
            _resultBase64 = prameter;
        }

        private async Task LaunchCommandExecute(object arg)
        {
            var bootStrap = new GeneralUpdateBootstrap();
            bootStrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
            bootStrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
            bootStrap.MutiDownloadError += OnMutiDownloadError;
            bootStrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
            bootStrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
            bootStrap.Exception += OnException;
            bootStrap.Option(UpdateOption.DownloadTimeOut, 30).
            Strategy<DefaultStrategy>().
            RemoteAddressBase64(_resultBase64);
            await bootStrap.LaunchTaskAsync();
        }

        private void OnMutiDownloadStatistics(object sender, GeneralUpdate.Core.Update.MutiDownloadStatisticsEventArgs e)
        {
            Tips1 = $"{ e.Speed }{ e.Remaining }";
        }

        private void OnMutiDownloadProgressChanged(object sender, GeneralUpdate.Core.Update.MutiDownloadProgressChangedEventArgs e)
        {
            Tips2 = $"{ e.ProgressValue }{ e.TotalBytesToReceive }";
        }

        private void OnMutiDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiDownloadCompletedEventArgs e)
        {
            Tips3 = $"{ e.Version.Name } download completed.";
        }

        private void OnMutiAllDownloadCompleted(object sender, GeneralUpdate.Core.Update.MutiAllDownloadCompletedEventArgs e)
        {
            if (e.IsAllDownloadCompleted)
            {
                Tips4 = "AllDownloadCompleted";
            }
            else
            {
                foreach (var version in e.FailedVersions)
                {
                    Debug.Write($"{ version.Item1.Name }");
                }
            }
        }

        private void OnMutiDownloadError(object sender, GeneralUpdate.Core.Update.MutiDownloadErrorEventArgs e)
        {
            Tips5 = $"{ e.Version.Name },{ e.Exception.Message }.";
        }

        private void OnException(object sender, GeneralUpdate.Core.Update.ExceptionEventArgs e)
        {
            Tips6 = $"{ e.Exception.Message }";
        }
    }
}
