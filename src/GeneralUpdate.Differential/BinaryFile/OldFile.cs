using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Differential.BinaryFile
{
    public class OldFile : IBinaryFile
    {
        public int OldFileIndex { set; get; }

        public int Length { set; get; }
    }
}
