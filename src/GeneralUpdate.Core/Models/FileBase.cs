using GeneralUpdate.Core.Update;

namespace GeneralUpdate.Core.Models
{
    public class FileBase : IFile
    {
        /// <summary>
        /// 文件名
        /// </summary>
        //public string Name { get; set; }

        /// <summary>
        /// MD5校验码
        /// </summary>
        //public string MD5 { get; set; }

        /// <summary>
        /// Client current version.
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// The latest version.
        /// </summary>
        public string LastVersion { get; set; }

        /// <summary>
        /// 文件唯一id
        /// </summary>
        //public string Guid { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        //public long Size { get; set; }

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        //public long? TotalSize { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        //public DateTime Date { get; set; }

        /// <summary>
        /// installation path (for update file logic).
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// Download file temporary storage path (for update file logic).
        /// </summary>
        public string TempPath { get; set; }
    }
}