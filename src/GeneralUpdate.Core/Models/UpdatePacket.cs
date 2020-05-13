using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Models
{
    public sealed class UpdatePacket : FileBase
    {
        public string MainApp { get; set; }

        /// <summary>
        /// 下载接收的大小
        /// </summary>
        public long ReceivedBytes { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public long? TotalBytes { get; set; }

        /// <summary>
        /// 进度值
        /// </summary>
        public int ProgressValue { get; set; }

        /// <summary>
        /// 更新包请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 更新包文件格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsForcibly { get; set; }
    }
}
