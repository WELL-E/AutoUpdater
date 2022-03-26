using Avalonia.Controls;
using GeneralUpdate.Differential;
using GeneralUpdate.PacketTool.Views;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;

namespace GeneralUpdate.PacketTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string sourcePath, targetPath, patchPath,infoMessage;
        public ReactiveCommand<Unit, Unit> BuildCommand { get; }
        public ReactiveCommand<object, Unit> SelectFolderCommand { get; }
        public string SourcePath { get => sourcePath; set => this.RaiseAndSetIfChanged(ref sourcePath, value); }
        public string TargetPath { get => targetPath; set => this.RaiseAndSetIfChanged(ref targetPath, value); }
        public string PatchPath { get => patchPath; set => this.RaiseAndSetIfChanged(ref patchPath, value); }
        public string InfoMessage { get => infoMessage; set => this.RaiseAndSetIfChanged(ref infoMessage,value); }

        public MainWindowViewModel()
        {
            BuildCommand = ReactiveCommand.Create(BuildPacketAction);
            SelectFolderCommand = ReactiveCommand.Create<object>(SelectFolderAction);
        }

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

        private void BuildPacketAction()
        {
            if (string.IsNullOrEmpty(SourcePath) || string.IsNullOrEmpty(TargetPath) || string.IsNullOrEmpty(PatchPath) || 
                !Directory.Exists(SourcePath) || !Directory.Exists(TargetPath) || !Directory.Exists(PatchPath)) 
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
                }
                catch (Exception ex)
                {
                    InfoMessage += $" Build failed : { TargetPath } , Error : { ex.Message }  !" + "\r\n";
                }
            });
        }
    }
}
