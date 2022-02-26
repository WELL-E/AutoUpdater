using System;
using System.Collections.Generic;

namespace GeneralUpdate.Common.Models
{
    [Serializable]
    public class ClientParameter
    {
        /// <summary>
        /// 1:ClientApp 2:UpdateApp
        /// </summary>
        public int ClientType { get; set; }

        /// <summary>
        /// Need to start the name of the app.
        /// </summary>
        public string AppName { get; set; }

        public string MainAppName { get; set; }

        /// <summary>
        /// Installation directory (the path where the update package is decompressed).
        /// </summary>
        public string InstallPath { get; set; }

        public string ClientVersion { get; set; }

        public string LastVersion { get; set; }

        /// <summary>
        /// Update log web address.
        /// </summary>
        public string UpdateLogUrl { get; set; }

        /// <summary>
        /// Whether to update.
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Update check api address.
        /// </summary>
        public string UpdateUrl { get; set; }

        /// <summary>
        /// Validate update url.
        /// </summary>
        public string ValidateUrl { get; set; }

        public string MainUpdateUrl { get; set; }

        public string MainValidateUrl { get; set; }

        /// <summary>
        /// One or more version update information.
        /// </summary>
        public List<UpdateVersion> UpdateVersions { get; set; }
    }

    [Serializable]
    public class UpdateVersion
    {
        public UpdateVersion(string md5, long pubTime, string version, string url, string name)
        {
            MD5 = md5;
            PubTime = pubTime;
            Version = version;
            Url = url;
            Name = name;
        }

        /// <summary>
        /// Update package release time.
        /// </summary>
        public long PubTime { get; set; }

        /// <summary>
        /// Update package name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Compare and verify with the downloaded update package.
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// The version number.
        /// </summary>
        public string Version { get; set; }

        public string Url { get; set; }

        public bool IsUnZip { get; set; }
    }
}