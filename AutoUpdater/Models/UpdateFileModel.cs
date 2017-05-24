
namespace AutoUpdater.Models
{
    internal class UpdateFileModel
    {
        private string _updateFileUrl;

        /// <summary>
        /// 当前版本
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 最新版本
        /// </summary>
        public string NewVersion { get; set; }

        /// <summary>
        /// 更新日志url
        /// </summary>
        public string UpdateLogUrl { get; set; }

        /// <summary>
        /// 更新文件地址
        /// </summary>
        public string UpdateFileUrl 
        {
            get { return _updateFileUrl; }
            set 
            {
                _updateFileUrl = value;
                var pos = _updateFileUrl.LastIndexOf('/');
                FileName = _updateFileUrl.Substring(pos + 1);
            }
        }

        /// <summary>
        /// 解压后文件存放路径
        /// </summary>
        public string UnpackPath { get; set; }

        /// <summary>
        /// 下载文件临时存储路径
        /// </summary>
        public string TempPath { get; set; }

        /// <summary>
        /// 更新包的md5
        /// </summary>
        public string FileMd5 { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载接收的大小
        /// </summary>
        public long ReceivedBytes { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public long? TotalBytes { get; set; }
    }
}
