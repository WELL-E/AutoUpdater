using GeneralUpdate.ClientCore.DTOs;
using GeneralUpdate.ClientCore.Models;
using GeneralUpdate.ClientCore.Utils;
using GeneralUpdate.ClientCore.Strategys;
using GeneralUpdate.ClientCore.Update;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using GeneralUpdate.ClientCore.Download;

namespace GeneralUpdate.ClientCore.Bootstrap
{
    public abstract class AbstractBootstrap<TBootstrap, TStrategy>
           where TBootstrap : AbstractBootstrap<TBootstrap, TStrategy>
           where TStrategy : IStrategy
    {
        #region Private Members

        private readonly ConcurrentDictionary<UpdateOption, UpdateOptionValue> options;
        private volatile Func<TStrategy> strategyFactory;
        private UpdatePacket _packet;
        private IStrategy strategy;
        private const string DefaultFormat = "zip";

        public delegate void MutiAllDownloadCompletedEventHandler(object sender, MutiAllDownloadCompletedEventArgs e);
        public event MutiAllDownloadCompletedEventHandler MutiAllDownloadCompleted;
        public delegate void MutiDownloadProgressChangedEventHandler(object sender, MutiDownloadProgressChangedEventArgs e);
        public event MutiDownloadProgressChangedEventHandler MutiDownloadProgressChanged;
        public delegate void MutiAsyncCompletedEventHandler(object sender, MutiDownloadCompletedEventArgs e);
        public event MutiAsyncCompletedEventHandler MutiDownloadCompleted;
        public delegate void MutiDownloadErrorEventHandler(object sender, MutiDownloadErrorEventArgs e);
        public event MutiDownloadErrorEventHandler MutiDownloadError;
        public delegate void MutiDownloadStatisticsEventHandler(object sender, MutiDownloadStatisticsEventArgs e);
        public event MutiDownloadStatisticsEventHandler MutiDownloadStatistics;
        public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);
        public event ExceptionEventHandler Exception;

        #endregion

        protected internal AbstractBootstrap()
        {
            this.options = new ConcurrentDictionary<UpdateOption, UpdateOptionValue>();
        }

        public UpdatePacket Packet
        {
            get { return _packet ?? (_packet = new UpdatePacket()); }
            set { _packet = value; }
        }

        #region Methods

        /// <summary>
        /// Launch udpate.
        /// </summary>
        /// <returns></returns>
        public virtual TBootstrap LaunchAsync()
        {
            Task.Run(async()=> 
            {
                await LaunchTaskAsync();
            });
            return (TBootstrap)this;
        }

        /// <summary>
        /// Launch udpate.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TBootstrap> LaunchTaskAsync()
        {
            try
            {
                MutiDownloadProgressChanged.Invoke(this,
                    new MutiDownloadProgressChangedEventArgs(null, ProgressType.Check, "Update checking..."));

                var updateResp = await HttpUtil.GetTaskAsync<UpdateVersionsRespDTO>(Packet.UpdateUrl);
                if (updateResp.Code == 200)
                {
                    var body = updateResp.Body;
                    Packet.UpdateVersions = ConvertUtil.ToUpdateVersions(body.UpdateVersions);
                    Packet.LastVersion = Packet.UpdateVersions[Packet.UpdateVersions.Count - 1].Version;
                }
                else
                {
                    MutiDownloadProgressChanged.Invoke(this,
                        new MutiDownloadProgressChangedEventArgs(null, ProgressType.Check, $"Check update failed :{ updateResp.Message }."));
                }

                if (Packet.UpdateVersions == null || Packet.UpdateVersions.Count == 0) throw new Exception("Request to update content failed!");

                var pacektFormat = GetOption(UpdateOption.Format) ?? DefaultFormat;
                Packet.Format = $".{pacektFormat}";
                Packet.AppName = GetOption(UpdateOption.MainApp) ?? Packet.AppName;
                Packet.TempPath = $"{ FileUtil.GetTempDirectory(Packet.LastVersion) }\\";
                var manager = new DownloadManager(Packet.TempPath, Packet.Format, GetOption(UpdateOption.DownloadTimeOut));
                manager.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
                manager.MutiDownloadCompleted += OnMutiDownloadCompleted;
                manager.MutiDownloadError += OnMutiDownloadError;
                manager.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
                manager.MutiDownloadStatistics += OnMutiDownloadStatistics;
                Packet.UpdateVersions.ForEach((version) =>
                {
                    manager.Add(new DownloadTask<UpdateVersion>(manager, version));
                });
                await manager.LaunchAsync();
            }
            catch (Exception ex)
            {
                this.Exception(this, new ExceptionEventArgs(ex));
                throw new Exception($"Launch error : { ex.Message }.");
            }
            return (TBootstrap)this;
        }

        #region Strategy

        protected IStrategy InitStrategy()
        {
            if (strategy == null)
            {
                Validate();
                strategy = this.strategyFactory();
                strategy.Create(Packet, MutiDownloadProgressAction, ExceptionAction);
            }
            return strategy;
        }

        protected IStrategy ExcuteStrategy()
        {
            var strategy = InitStrategy();
            strategy.Excute();
            return strategy;
        }

        public virtual TBootstrap Validate()
        {
            if (this.strategyFactory == null)
            {
                throw new InvalidOperationException("Strategy or strategy factory not set.");
            }
            return (TBootstrap)this;
        }

        public virtual TBootstrap Strategy<T>() where T : TStrategy, new() => this.StrategyFactory(() => new T());

        public TBootstrap StrategyFactory(Func<TStrategy> strategyFactory)
        {
            Contract.Requires(strategyFactory != null);
            this.strategyFactory = strategyFactory;
            return (TBootstrap)this;
        }

        #endregion

        #region Config option.

        public virtual TBootstrap Option<T>(UpdateOption<T> option, T value)
        {
            Contract.Requires(option != null);
            if (value == null)
            {
                this.options.TryRemove(option, out UpdateOptionValue removed);
            }
            else
            {
                this.options[option] = new UpdateOptionValue<T>(option, value);
            }
            return (TBootstrap)this;
        }

        public virtual T GetOption<T>(UpdateOption<T> option)
        {
            if (options == null || options.Count == 0) return default(T);

            var val = options[option];
            if (val != null)
            {
                return (T)val.GetValue();
            }
            return default(T);
        }

        #endregion

        #region Callback event.

        private void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            if (MutiDownloadStatistics != null)
                MutiDownloadStatistics.Invoke(this,e);
        }

        protected void MutiDownloadProgressAction(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            if (MutiDownloadProgressChanged != null)
                MutiDownloadProgressChanged.Invoke(sender, e);
        }

        protected void ExceptionAction(object sender, ExceptionEventArgs e)
        {
            if (Exception != null)
                Exception.Invoke(this, e);
        }

        private void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            if (MutiDownloadProgressChanged != null)
                MutiDownloadProgressChanged.Invoke(this, e);
        }

        private void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            if (MutiDownloadCompleted != null)
                MutiDownloadCompleted.Invoke(sender, e);
        }

        private void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            if (MutiDownloadError != null)
                MutiDownloadError.Invoke(this, e);
        }

        private void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            try
            {
                if(e.FailedVersions.Count > 0) throw new Exception("Failed to execute strategy, You need to view the 'FailedVersions' in the error notification event !", null);

                if (MutiAllDownloadCompleted != null)
                    MutiAllDownloadCompleted.Invoke(this, e);

                ExcuteStrategy();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute strategy!", ex);
            }
        }
        
        #endregion

        #endregion
    }
}
