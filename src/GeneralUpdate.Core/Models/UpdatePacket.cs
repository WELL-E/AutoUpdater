using GeneralUpdate.Common.Models;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Models
{
    public enum AppType
    {
        /// <summary>
        /// main program
        /// </summary>
        ClientApp = 1,

        /// <summary>
        /// upgrade program.
        /// </summary>
        UpdateApp = 2,
    }

    public sealed class UpdatePacket : FileBase
    {
        /// <summary>
        /// Update check api address.
        /// </summary>
        public string MainUpdateUrl { get; set; }

        /// <summary>
        /// Validate update url.
        /// </summary>
        public string MainValidateUrl { get; set; }

        /// <summary>
        /// 1:ClientApp 2:UpdateApp
        /// </summary>
        public int AppType { get; set; }

        /// <summary>
        /// Update check api address.
        /// </summary>
        public string UpdateUrl { get; set; }

        /// <summary>
        /// Validate update url.
        /// </summary>
        public string ValidateUrl { get; set; }

        /// <summary>
        /// Need to start the name of the app.
        /// </summary>
        public string AppName { get; set; }

        public string MainAppName { get; set; }

        /// <summary>
        /// Update package file format(Defult format is Zip).
        /// </summary>
        public string CompressFormat { get; set; }

        /// <summary>
        /// Whether to force update.
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Update log web address.
        /// </summary>
        public string UpdateLogUrl { get; set; }

        /// <summary>
        /// Version information that needs to be updated.
        /// </summary>
        public List<UpdateVersion> UpdateVersions { get; set; }

        public Encoding CompressEncoding { get; set; }

        public int DownloadTimeOut { get; set; }
    }
}