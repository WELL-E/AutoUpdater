using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Utils
{
    public class GeneralWebClient : WebClient
    {
        private DownloadFileRangeState state;
        private long _beforBytes;
        private long _receivedBytes;
        private long _totalBytes;
        public string _url;
        public int _timeOut;
        public delegate void DownloadProgressChangedEventHandlerEx(object sender, DownloadProgressChangedEventArgsEx e);
        public event DownloadProgressChangedEventHandlerEx DownloadProgressChangedEx;
        public delegate void AsyncCompletedEventHandlerEx(object sender, AsyncCompletedEventArgs e);
        public event AsyncCompletedEventHandlerEx DownloadFileCompletedEx;

        protected Timer SpeedTimer { get; set; }
        protected DateTime StartTime { get; set; }
        public UpdateVersion Version { get; set; }

        public long BeforBytes 
        {
            get 
            {
                return Interlocked.Read(ref _beforBytes);
            }

            set 
            {
                Interlocked.Exchange(ref _beforBytes, value);
            }
        }

        public long ReceivedBytes 
        {
            get 
            { 
                return Interlocked.Read(ref _receivedBytes);
            }

            set 
            {
                Interlocked.Exchange(ref _receivedBytes,value);
            }
        }

        public long TotalBytes 
        {
            get 
            { 
                return Interlocked.Read(ref _totalBytes);
            }
            set 
            {
                Interlocked.Exchange(ref _totalBytes, value);
            }
        }

        public void InitTimeOut(int timeout)
        {
            if (timeout == 0) timeout = 30;
            _timeOut = 1000 * timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request;
            if (address.Scheme == "https")
            {
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
                request = (HttpWebRequest)base.GetWebRequest(address);
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = (HttpWebRequest)base.GetWebRequest(address);
            }

            request.Timeout = _timeOut;
            request.ReadWriteTimeout = _timeOut;
            _url = address.OriginalString;
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = true;

            CookieContainer cookieContainer = new CookieContainer();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("Accept-Language", "zh-cn,zh;q=0.5");
            collection.Add("Accept-Encoding", "gzip,deflate");
            collection.Add("Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7");
            collection.Add("Keep-Alive", "115");
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
            request.Headers.Add(collection);
            request.CookieContainer = cookieContainer;

            request.ServicePoint.BindIPEndPointDelegate = (servicePoint, remoteEndPoint, retryCount) =>
            {
                if (remoteEndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    return new IPEndPoint(IPAddress.IPv6Any, 0);
                else
                    return new IPEndPoint(IPAddress.Any, 0);
            };

            return request;
        }

        public new void CancelAsync()
        {
            base.CancelAsync();
            if (state != null && state._isRangeDownload) state._isRangeDownload = false;
        }

        public Task DownloadFileRangeTaskAsync(string url, string path, object userState)
        {
            return Task.Run(() => {
                DownloadFileRange(url, path, userState);
            });
        }

        public void DownloadFileRange(string url, string path, object userState)
        {
            if (state != null && state._isRangeDownload) return;

            state = new DownloadFileRangeState(path, userState, this);
            state.onCompleted = () => DownloadFileCompletedEx;
            state._isRangeDownload = true;
            long startPos = CheckFile(state);
            if (startPos == -1) return;

            try
            {
                state._request = (HttpWebRequest)GetWebRequest(new Uri(url));
                state._request.ReadWriteTimeout = _timeOut;
                state._request.Timeout = _timeOut;
                if (startPos > 0) state._request.AddRange((int)startPos);
                state._respone = state._request.GetResponse();
                state._stream = state._respone.GetResponseStream();
                long totalBytesReceived = state._respone.ContentLength + startPos;
                long bytesReceived = startPos;
                if (totalBytesReceived != 0 && bytesReceived >= totalBytesReceived)
                {
                    state.Close();
                    try
                    {
                        if (File.Exists(state._path)) File.Delete(state._path);
                        File.Move(state._tempPath, state._path);
                    }
                    catch (Exception e)
                    {
                        state._exception = e;
                        state.Close();
                    }
                }
                else
                {
                    WriteFile(state, startPos);
                }
            }
            catch (Exception e)
            {
                state._exception = e;
            }
            finally
            {
                if (state != null)
                {
                    state.Close();
                }
            }
        }

        private long CheckFile(DownloadFileRangeState state)
        {
            long startPos = 0;
            if (File.Exists(state._tempPath))
            {
                state._fileStream = File.OpenWrite(state._tempPath);
                startPos = state._fileStream.Length;
                state._fileStream.Seek(startPos, SeekOrigin.Current);
            }
            else
            {
                try
                {
                    string direName = Path.GetDirectoryName(state._tempPath);
                    if (!Directory.Exists(direName)) Directory.CreateDirectory(direName);
                    state._fileStream = new FileStream(state._tempPath, FileMode.Create);
                }
                catch (Exception e)
                {
                    state._exception = e;

                    startPos = -1;
                    state.Close();
                }
            }
            return startPos;
        }

        private void WriteFile(DownloadFileRangeState state, long startPos)
        {
            var bytesReceived = startPos;
            byte[] bytes = new byte[1024];
            bool isDownloadCompleted = false;
            var totalBytesReceived = state._respone.ContentLength + startPos;

            int readSize = state._stream.Read(bytes, 0, 1024);

            while (readSize > 0 && state._isRangeDownload)
            {
                if (state == null || state._fileStream == null) break;

                lock (state._fileStream)
                {
                    if (DownloadProgressChangedEx != null)
                        DownloadProgressChangedEx(this, new DownloadProgressChangedEventArgsEx(bytesReceived, totalBytesReceived, ((float)bytesReceived / totalBytesReceived), state._userState));

                    state._fileStream.Write(bytes, 0, readSize);//Write temp file.
                    bytesReceived += readSize;
                    if (totalBytesReceived != 0 && bytesReceived >= totalBytesReceived)
                    {
                        try
                        {
                            state.Close();
                            if (File.Exists(state._path)) File.Delete(state._path);
                            File.Move(state._tempPath, state._path);
                            isDownloadCompleted = true;
                            state.Done(isDownloadCompleted);
                        }
                        catch (Exception e)
                        {
                            state._exception = e;
                            state.Done(false);
                        }
                    }
                    else
                    {
                        readSize = state._stream.Read(bytes, 0, 1024);
                    }
                }
            }

            if (!isDownloadCompleted)
            {
                state._exception = new Exception("Request for early closure");
            }
        }

        private class DownloadFileRangeState
        {
            private readonly string tmpSuffix = ".temp";
            public Func<AsyncCompletedEventHandlerEx> onCompleted;
            public HttpWebRequest _request = null;
            public WebResponse _respone = null;
            public Stream _stream = null;
            public FileStream _fileStream = null;
            public Exception _exception = null;
            public bool _isRangeDownload;
            public string _tempPath;
            public string _path;
            public object _userState;
            public object _sender;

            public DownloadFileRangeState(string path, object userState, object sender)
            {
                this._path = path;
                this._userState = userState;
                _tempPath = path + tmpSuffix;
                this._sender = sender;
            }

            public void Close()
            {
                if (_fileStream != null)
                {
                    _fileStream.Flush();
                    _fileStream.Close();
                    _fileStream = null;
                }
                if (_stream != null) _stream.Close();
                if (_respone != null) _respone.Close();
                if (_request != null) _request.Abort();

                //if (_exception != null) throw new Exception(_exception.Message);
            }

            public void Done(bool isCompleted)
            {
                if (_exception != null) throw new Exception(_exception.Message);
                if (onCompleted() != null)
                    onCompleted()(_sender, new AsyncCompletedEventArgs(_exception, isCompleted, _userState));
            }
        }
    }

    public sealed class GeneralMutiWebClient : GeneralWebClient
    {
        /// <summary>
        /// Number of failed download tasks.
        /// </summary>
        private int _failed;

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

        private IList<ValueTuple<UpdateVersion, string>> FailedVersions { get; set; }
        private List<UpdateVersion> UpdateVersionInfos { get; set; }
        private string Path { get; set; }
        private string Format { get; set; }

        private const int DEFAULT_DELTA = 1048576;//1024*1024

        public GeneralMutiWebClient(List<UpdateVersion> updateVersionInfos, string path, string format)
        {
            Path = path;
            Format = format;
            UpdateVersionInfos = updateVersionInfos;
            FailedVersions = new List<ValueTuple<UpdateVersion, string>>();
        }

        private GeneralMutiWebClient() : base() { }

        public void MutiDownloadAsync()
        {
            if (UpdateVersionInfos == null || UpdateVersionInfos.Count == 0) return;
            var downloadTasks = new List<Task>();
            UpdateVersionInfos.ForEach(version =>
            {
                var downloadTask = DownloadTaskAsync(version);
                downloadTasks.Add(downloadTask);
            });
            try
            {
                Task.WaitAll(downloadTasks.ToArray());
            }
            catch (Exception ex)
            {
                MutiDownloadError(this, new MutiDownloadErrorEventArgs(ex, null));
                throw new Exception($"Muti download error: { ex.Message }, { ex.StackTrace }.");
            }
            finally
            {
                MutiAllDownloadCompleted(this, new MutiAllDownloadCompletedEventArgs(_failed == 0, FailedVersions));
            }
        }

        private async Task<bool> DownloadTaskAsync(UpdateVersion version)
        {
            GeneralMutiWebClient webClient = null;
            try
            {
                webClient = new GeneralMutiWebClient();
                if (webClient.SpeedTimer == null)
                {
                    webClient.SpeedTimer = new Timer((state) =>
                    {
                        if (webClient == null) return;

                        var interval = DateTime.Now - webClient.StartTime;

                        var downLoadSpeed = interval.Seconds < 1
                            ? StatisticsUtil.ToUnit(webClient.ReceivedBytes - webClient.BeforBytes)
                            : StatisticsUtil.ToUnit(webClient.ReceivedBytes - webClient.BeforBytes / interval.Seconds);

                        var size = (webClient.TotalBytes - webClient.ReceivedBytes) / DEFAULT_DELTA;
                        var remainingTime = new DateTime().AddSeconds(Convert.ToDouble(size));

                        MutiDownloadStatistics.Invoke(this,
                            new MutiDownloadStatisticsEventArgs { Version = webClient.Version , Remaining = remainingTime, Speed = downLoadSpeed });

                        webClient.StartTime = DateTime.Now;
                        webClient.BeforBytes = webClient.ReceivedBytes;
                    }, null, 0, 1000);
                }

                webClient.DownloadProgressChangedEx += new DownloadProgressChangedEventHandlerEx((sender, e) =>
                {
                    webClient.ReceivedBytes = e.BytesReceived;
                    webClient.TotalBytes = e.TotalBytesToReceive;
                    
                    var eventArgs = new MutiDownloadProgressChangedEventArgs(webClient.Version,
                        ProgressType.Donwload,
                        string.Empty,
                        e.BytesReceived / DEFAULT_DELTA,
                        e.TotalBytesToReceive / DEFAULT_DELTA,
                        e.ProgressPercentage,
                        e.UserState);

                    if (MutiDownloadProgressChanged != null) MutiDownloadProgressChanged(sender, eventArgs);
                });

                webClient.DownloadFileCompletedEx += new AsyncCompletedEventHandlerEx((sender, e) =>
                {
                    try
                    {
                        if (webClient.SpeedTimer != null)
                        {
                            webClient.SpeedTimer.Dispose();
                            webClient.SpeedTimer = null;
                        }

                        if (webClient != null)
                        {
                            var eventArgs = new MutiDownloadCompletedEventArgs(webClient.Version, e.Error, e.Cancelled, e.UserState);
                            if (MutiDownloadCompleted != null)
                                MutiDownloadCompleted(sender, eventArgs);

                            webClient.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Error(ex, webClient.Version);
                    }
                });
                webClient.InitTimeOut(_timeOut);
                webClient.Version = version;
                var installPath = $"{Path}\\{ version.Name }{ Format }";
                await webClient.DownloadFileRangeTaskAsync(version.Url, installPath, null);
                return true;
            }
            catch (WebException webException)
            {
                Error(webException, version);
                return false;
            }
            catch (Exception exception)
            {
                Error(exception, version);
                return false;
            }
        }

        private void Error(Exception exception, UpdateVersion version)
        {
            Interlocked.Increment(ref _failed);
            FailedVersions.Add(new ValueTuple<UpdateVersion, string>(version, exception.Message));
            MutiDownloadError(this, new MutiDownloadErrorEventArgs(exception, version));
        }
    }
}