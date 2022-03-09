using System.Collections.Generic;

namespace GeneralUpdate.Differential.BinaryFile
{
    public class NewFile : IBinaryFile
    {
        public List<byte> Data { set; get; } = new List<byte>();
    }
}