using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Strategys.PlatformAndroid;
using GeneralUpdate.Core.Strategys.PlatformMac;
using GeneralUpdate.Core.Strategys.PlatformWindows;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Strategys
{
    public sealed class StrategyFactory : AbstractStrategy
    {
        private IStrategy _strategyInstance;
        private const string PATCHS = "patchs";
        protected UpdatePacket Packet { get; set; }
        protected Action<object, MutiDownloadProgressChangedEventArgs> ProgressEventAction { get; set; }
        protected Action<object, ExceptionEventArgs> ExceptionEventAction { get; set; }

        public override void Create(string platformType, IFile file, Action<object, MutiDownloadProgressChangedEventArgs> progressEventAction, Action<object, ExceptionEventArgs> exceptionEventAction)
        {
            Packet = (UpdatePacket)file;
            ProgressEventAction = progressEventAction;
            ExceptionEventAction = exceptionEventAction;
            switch (platformType)
            {
                case PlatformType.Windows:
                    _strategyInstance = new WindowsStrategy();
                    break;
                case PlatformType.Mac:
                    _strategyInstance = new MacStrategy();
                    break;
                case PlatformType.Android:
                    _strategyInstance = new AndroidStrategy();
                    break;
                case PlatformType.iOS:
                    _strategyInstance = new PlatformiOS.iOSStrategy();
                    break;
            }
        }

        public override void Excute()
        {
            if (_strategyInstance == null) throw new NullReferenceException(nameof(_strategyInstance));
            _strategyInstance.Excute();
        }

        protected override bool StartApp(string appName)
        {
            return base.StartApp(appName);
        }
    }
}
