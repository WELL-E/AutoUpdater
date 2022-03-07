using GeneralUpdate.Differential.BinaryFile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential
{
    public class DifferentialCore
    {
        public async Task<bool> Launch(byte[] oldfileArrary , byte[] newfileArray) 
        {
            return await Task.Run(() => 
            {
                List<IBinaryFile> file = Binary.GetBinary(oldfileArrary, newfileArray);
                file = Binary.Deserialize(Binary.Serialize(file));
                return Binary.Equals(newfileArray, new Span<byte>(Binary.GetNewFile(oldfileArrary, file).ToArray()));
            });
        }
    }
}
