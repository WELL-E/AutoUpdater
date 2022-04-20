using Avalonia.Controls;
using GeneralUpdate.Differential;
using GeneralUpdate.PacketTool.Utils;
using GeneralUpdate.PacketTool.Views;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;

namespace GeneralUpdate.PacketTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string sourcePath, targetPath, patchPath,infoMessage;
        private bool isPublish;
        private string url;

        public ReactiveCommand<Unit, Unit> BuildCommand { get; }
        public ReactiveCommand<object, Unit> SelectFolderCommand { get; }
        public string SourcePath { get => sourcePath; set => this.RaiseAndSetIfChanged(ref sourcePath, value); }
        public string TargetPath { get => targetPath; set => this.RaiseAndSetIfChanged(ref targetPath, value); }
        public string PatchPath { get => patchPath; set => this.RaiseAndSetIfChanged(ref patchPath, value); }
        public string InfoMessage { get => infoMessage; set => this.RaiseAndSetIfChanged(ref infoMessage,value); }
        public bool IsPublish { get => isPublish; set => this.RaiseAndSetIfChanged(ref isPublish, value); }
        public string Url { get => url; set => this.RaiseAndSetIfChanged(ref url, value); }

        public MainWindowViewModel()
        {
            IsPublish = false;
            BuildCommand = ReactiveCommand.Create(BuildPacketAction);
            SelectFolderCommand = ReactiveCommand.Create<object>(SelectFolderAction);
        }

        /// <summary>
        /// Choose a path
        /// </summary>
        /// <param name="obj"></param>
        private async void SelectFolderAction(object obj)
        {
            var button = obj as Button;
            var dialog = new OpenFolderDialog();
            var result =  await dialog.ShowAsync(MainWindow.Instance);
            if (result == null || button == null) return;
            switch (button.Name)
            {
                case "BtnSource":
                    SourcePath = result;
                break;
                case "BtnTarget":
                    TargetPath = result;
                    break;
                case "BtnPatch":
                    PatchPath = result;
                    break;
            }
        }

        /// <summary>
        ///  Build patch package
        /// </summary>
        private void BuildPacketAction()
        {
            if (ValidationParameters()) 
            {
                InfoMessage += "The path is not set or the folder does not exist !" + "\r\n";
                return;
            }
            Task.Run(async () =>
            {
                try
                {
                    await DifferentialCore.Instance.Clean(SourcePath, TargetPath, PatchPath, (sender, args) =>
                    {
                        InfoMessage += $"{args.Name} - {args.Path}" + "\r\n";
                    });
                    InfoMessage += $" Build succeeded : { TargetPath } ." + "\r\n";
                    //If upload is checked, the differential package will be uploaded to the file server,
                    //and the file server will insert the information of the update package after receiving it.
                    //TODO: Still need to design.
                    if (IsPublish) 
                    {
                        var directoryInfo = new DirectoryInfo(TargetPath);
                        var fileArray = directoryInfo.GetFiles();
                        var findPacket = fileArray.FirstOrDefault(f=>f.Extension.Equals(".zip"));
                        if (findPacket == null) return;
                        InfoMessage += $" Update package found under { TargetPath }  path , file full name { findPacket.Name } ." + "\r\n";
                        var httpClient = new HttpClient();
                        var responseMessage = await httpClient.PostFileAsync(Url, findPacket.FullName);
                        InfoMessage = responseMessage.StatusCode == HttpStatusCode.OK ? $" Successfully published to the server { Url } ." + "\r\n" : $" Failed to publish to server { Url } !" + "\r\n";
                        IsPublish = !(responseMessage.StatusCode == HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    InfoMessage += $"Operation failed : { TargetPath } , Error : { ex.Message }  !" + "\r\n";
                }
            });
        }

        /// <summary>
        /// parameter validation
        /// </summary>
        /// <returns></returns>
        private bool ValidationParameters()
        {
            if (string.IsNullOrEmpty(SourcePath) || string.IsNullOrEmpty(TargetPath) || string.IsNullOrEmpty(PatchPath) ||
                !Directory.Exists(SourcePath) || !Directory.Exists(TargetPath) || !Directory.Exists(PatchPath)) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
