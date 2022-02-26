using GeneralUpdate.Core.Update;
using System;

namespace GeneralUpdate.Core.Models
{
    public class FileBase : IFile
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// MD5校验码
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 最新版本
        /// </summary>
        public string LastVersion { get; set; }

        /// <summary>
        /// 文件唯一id
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public long? TotalSize { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 安装路径（用于更新文件逻辑）
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// 下载文件临时存储路径（用于更新文件逻辑）
        /// </summary>
        public string TempPath { get; set; }

        /// <summary>
        /// 文件当前路径（用于增量文件逻辑）
        /// </summary>
        public string Path { get; set; }
    }
}