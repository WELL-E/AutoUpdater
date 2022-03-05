using GeneralUpdate.Core.Config.Handles;

namespace GeneralUpdate.Core.Config.Cache
{
    public class ConfigEntity
    {
        /// <summary>
        /// file md5 code .
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// configuation file content.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// configuration file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// handle type (.json .ini .xml .db) .
        /// </summary>
        public HandleEnum Handle { get; set; }
    }
}