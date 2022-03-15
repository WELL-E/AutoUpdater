using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Differential.Common
{
    public class Filefilter
    {
        public readonly static List<string> All = new List<string>() { ".db", ".xml", ".ini", ".json" , ".config" };

        public readonly static List<string> Temp = new List<string>() { ".json" };

        public readonly static List<string> Diff = new List<string>() { ".patch" , ".7z" , ".zip" , ".json" };
    }
}
