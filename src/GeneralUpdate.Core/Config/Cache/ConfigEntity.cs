using GeneralUpdate.Core.Config.Handles;

namespace GeneralUpdate.Core.Config.Cache
{
    public class ConfigEntity
    {
        public string MD5 { get; set; }

        public object Content { get; set; }

        public string Path { get; set; }

        public HandleEnum Handle { get; set; }
    }
}