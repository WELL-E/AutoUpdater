using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Pipelines.Context
{
    public class UpdateContext
    {
        public Version VersionInfo { get; set; }

        public string TargetPath { get; set; }

        public string SourcePath { get; set; }
    }
}
