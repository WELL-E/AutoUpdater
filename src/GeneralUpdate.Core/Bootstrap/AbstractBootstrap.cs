using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading;

namespace GeneralUpdate.Core.Bootstrap
{
    public abstract class AbstractBootstrap<TBootstrap, TStrategy>
           where TBootstrap : AbstractBootstrap<TBootstrap, TStrategy>
           where TStrategy : IStrategy
    {
        #region Private Members

        private Timer _speedTimer;
        private readonly ConcurrentDictionary<UpdateOption, UpdateOptionValue> options;
        private volatile Func<TStrategy> strategyFactory;
        private readonly WebClient webClient;
        private DateTime _startTime;
        private UpdatePacket _packet;
        private IStrategy strategy;
        private const string DefultFormat = "zip";

        public delegate void DownloadStatisticsEventHandler(object sender, DownloadStatisticsEventArgs e);
        /// <summary>
        /// 下载统计
        /// </summary>
        public event DownloadStatisticsEventHandler DownloadStatistics;

        public delegate void ProgressChangedEventHandler(object sender, Update.ProgressChangedEventArgs e);
        /// <summary>
        /// 进度更新
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        #endregion

        #region Constructors

        protected internal AbstractBootstrap()
        {
            _startTime = DateTime.Now;
            this.options = new ConcurrentDictionary<UpdateOption, UpdateOptionValue>();
            this.webClient = new WebClient();
            webClient.DownloadProgressChanged += OnDownloadProgressChanged;
            webClient.DownloadFileCompleted += OnDownloadFileCompleted;
        }

        #endregion

        #region Public Properties

        public UpdatePacket Packet
        {
            get { return _packet ?? (_packet = new UpdatePacket()); }
            set { _packet = value; }
        }

        public string UpdateCheckUrl { get; set; }

        public long BeforBytes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 启动更新
        /// </summary>
        /// <returns></returns>
        public virtual TBootstrap Launch()
        {
            if (!string.IsNullOrWhiteSpace(UpdateCheckUrl))
            {
                ProgressChanged(this,new Update.ProgressChangedEventArgs { Type = ProgressType.Check , Message = "更新检查！" });
                var info = HttpUtil.GetAsync<UpdateInfoHttpResp>(UpdateCheckUrl).Result;
                if (info.Code == 200)
                {
                    var result = info.Result;
                    Packet.NewVersion = result.Version;
                    Packet.MD5 = result.HashCode;
                    Packet.Url = result.Url;
                    var pos = Packet.Url.LastIndexOf('/');
                    Packet.Name = Packet.Url.Substring(pos + 1);
                }
                else
                {
                    ProgressChanged(this, new Update.ProgressChangedEventArgs { Type = ProgressType.Check, Message = $"check update fail:{ info.Message }" });
                }
            }
            var pacektFormat = GetOption(UpdateOption.Format) ?? DefultFormat;
            Packet.Format = $".{pacektFormat}";
            Packet.MainApp = GetOption(UpdateOption.MainApp);
            _speedTimer = new Timer(SpeedTimerOnTick, null, 0, 1000);
            webClient.DownloadFileAsync(new Uri(Packet.Url), $"{Packet.TempPath}");
            return (TBootstrap)this;
        }

        #region 策略

        protected IStrategy InitStrategy()
        {
            if (strategy == null)
            {
                Validate();
                strategy = this.strategyFactory();
                strategy.Create(Packet, DoProgressChanged);
            }
            return strategy;
        }

        IStrategy ExcuteStrategy()
        {
            var strategy = InitStrategy();
            strategy.Excute();
            return strategy;
        }

        public virtual TBootstrap Validate()
        {
            if (this.strategyFactory == null)
            {
                throw new InvalidOperationException("strategy or strategyFactory not set");
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

        #region 配置操作

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
            var val = options[option];
            if (val != null)
            {
                return (T)val.GetValue();
            }
            return default;
        }

        #endregion

        #region 事件回调函数

        protected void DoProgressChanged(object sender, Update.ProgressChangedEventArgs eventArgs)
        {
            ProgressChanged(sender, eventArgs);
        }

        /// <summary>
        /// 下载速度
        /// </summary>
        /// <param name="sender"></param>
        private void SpeedTimerOnTick(object sender)
        {
            var interval = DateTime.Now - _startTime;

            var downLoadSpeed = interval.Seconds < 1
                ? StatisticsUtil.ToUnit(Packet.ReceivedBytes - BeforBytes)
                : StatisticsUtil.ToUnit(Packet.ReceivedBytes - BeforBytes / interval.Seconds);

            var size = (Packet.TotalBytes - Packet.ReceivedBytes) / (1024 * 1024);
            var remainingTime = new DateTime().AddSeconds(Convert.ToDouble(size));

            var args = new DownloadStatisticsEventArgs();
            args.Remaining = remainingTime;
            args.Speed = downLoadSpeed;
            DownloadStatistics(this, args);

            _startTime = DateTime.Now;
            BeforBytes = Packet.ReceivedBytes;
        }

        /// <summary>
        /// 下载进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Packet.ReceivedBytes = e.BytesReceived;
            Packet.TotalBytes = e.TotalBytesToReceive;

            var args = new Update.ProgressChangedEventArgs();
            args.ProgressValue = e.ProgressPercentage;
            args.ReceivedSize = e.BytesReceived / (1024 * 1024);
            args.TotalSize = e.TotalBytesToReceive / (1024 * 1024);
            args.Type = ProgressType.Donwload;
            ProgressChanged(this, args);
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (_speedTimer != null)
                {
                    _speedTimer.Dispose();
                    _speedTimer = null;
                }
            }
            catch (Exception ex)
            {
                ProgressChanged(this, new Update.ProgressChangedEventArgs { Type = ProgressType.Check, Message = $"Dispose timer error:{ ex.Message }." });
            }
            finally 
            {
                ExcuteStrategy();
            }
        }

        #endregion

        #endregion
    }
}
