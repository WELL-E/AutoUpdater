using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Update
{
    public enum ProgressType {
        /// <summary>
        /// 检查更新
        /// </summary>
        Check,
        /// <summary>
        /// 下载更新包
        /// </summary>
        Donwload,
        /// <summary>
        /// 更新文件
        /// </summary>
        Updatefile,
        /// <summary>
        /// 更新完成
        /// </summary>
        Done,
        /// <summary>
        /// 更新失败
        /// </summary>
        Fail
    }

    public class ProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 进度状态
        /// </summary>
        public ProgressType Type { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public double ProgressValue { get; set; }

        /// <summary>
        /// 已下载文件大小
        /// </summary>
        public double ReceivedSize { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public double? TotalSize { get; set; }

        public string Message { get; set; }
    }


    /// <summary>
    /// 下载信息统计
    /// </summary>
    public class DownloadStatisticsEventArgs : EventArgs
    {

        public DateTime Remaining { get; set; }

        public string Speed { get; set; }
    }
}
