﻿using GeneralUpdate.Core.Update;

namespace GeneralUpdate.Core.Models
{
    public class FileBase : IFile
    {
        /// <summary>
        /// Client current version.
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// The latest version.
        /// </summary>
        public string LastVersion { get; set; }

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