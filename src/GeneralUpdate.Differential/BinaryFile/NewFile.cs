using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Differential.BinaryFile
{
    public class NewFile : IBinaryFile
    {
        public List<byte> Data { set; get; } = new List<byte>();
    }
}
