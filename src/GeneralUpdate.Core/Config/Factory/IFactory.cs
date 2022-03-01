using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Config.Factory
{
    public interface IFactory
    {
        /// <summary>
        /// Cache configuration file content.
        /// </summary>
        void Cache();

        /// <summary>
        /// Scan all configuration files in the current directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configFormat"></param>
        /// <returns></returns>
        IEnumerable<string> Scan(string path,IEnumerable<string> configFormat);

        /// <summary>
        /// Create a temporary configuration folder in the current directory after caching is complete and delete it after updating.
        /// </summary>
        void Backup();

        /// <summary>
        /// Update configuration file content and delete backup or redundant files.
        /// </summary>
        void Deploy();

        /// <summary>
        /// Verify that the current configuration file matches the content fields of the file to be updated.
        /// </summary>
        /// <returns></returns>
        bool Verify();
    }
}
