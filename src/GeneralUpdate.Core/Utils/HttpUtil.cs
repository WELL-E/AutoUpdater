using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Utils
{
    public class HttpUtil
    {
        public static async Task<T> GetAsync<T>(string http_url, string header_key = null, string header_value = null)
        {
            HttpWebResponse response = null;
            try
            {
                string httpUri = http_url;
                var encoding = Encoding.GetEncoding("utf-8");
                var request = (HttpWebRequest)WebRequest.Create(httpUri);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 15000;
                if (!string.IsNullOrEmpty(header_key) && !string.IsNullOrEmpty(header_value))
                {
                    request.Headers[header_key] = header_value;
                }
                response = (HttpWebResponse)await request.GetResponseAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        var tempStr = reader.ReadToEnd();
                        var respContent = JsonConvert.DeserializeObject<T>(tempStr);
                        return respContent;
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            { 
                return default(T);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }

    public class DownloadProgressChangedEventArgsEx
    {
        public long BytesReceived { get; private set; }
        public long TotalBytesToReceive { get; private set; }
        public float ProgressPercentage { get; private set; }
        public object UserState;
        public DownloadProgressChangedEventArgsEx(long received, long toReceive, float progressPercentage, object userState)
        {
            BytesReceived = received;
            TotalBytesToReceive = toReceive;
            ProgressPercentage = progressPercentage;
            UserState = userState;
        }
    }

    public sealed class GeneralWebClient : WebClient
    {
        public string _url;
        public int _timeOut;
        private DownloadFileRangeState state;
        public delegate void DownloadProgressChangedEventHandlerEx(object sender, DownloadProgressChangedEventArgsEx e);
        public event DownloadProgressChangedEventHandlerEx DownloadProgressChangedEx;
        public delegate void AsyncCompletedEventHandlerEx(object sender, AsyncCompletedEventArgs e);
        public event AsyncCompletedEventHandlerEx DownloadFileCompletedEx;

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

        public void DownloadFileRangeAsync(string url, string path, object userState)
        {
            if (state != null && state._isRangeDownload)
                return;

            Task.Run(() => {
                state = new DownloadFileRangeState(path, userState);
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
            });
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


            public DownloadFileRangeState(string path, object userState)
            {
                this._path = path;
                this._userState = userState;
                _tempPath = path + tmpSuffix;
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

                if (_exception != null) throw new Exception(_exception.Message);
            }

            public void Done(bool isCompleted) 
            {
                if (_exception != null) throw new Exception(_exception.Message);
                if (onCompleted() != null)
                    onCompleted()(this, new AsyncCompletedEventArgs(_exception, isCompleted, _userState));
            }
        }
    }
}
