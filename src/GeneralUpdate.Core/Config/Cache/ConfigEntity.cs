using GeneralUpdate.Common.Models;
using GeneralUpdate.Core.Config.Handles;
using System;
using System.Collections.Generic;
using System.Text;

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
