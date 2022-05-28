using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Differential.Common
{
    public class FolderDepend
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public FolderDepend(string name , string path) 
        {
            Name = name;
            FullName = path;
        }
    }
}
