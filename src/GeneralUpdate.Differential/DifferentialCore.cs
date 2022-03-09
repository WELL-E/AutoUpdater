using GeneralUpdate.Differential.BinaryFile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential
{
    public class DifferentialCore
    {
        public async Task<bool> Launch(byte[] oldfileArrary, byte[] newfileArray)
        {
            return await Task.Run(() =>
            {
                Binary binary = new Binary();
                List<IBinaryFile> file = binary.GetBinary(oldfileArrary, newfileArray);
                file = binary.Deserialize(binary.Serialize(file));
                return binary.Equals(newfileArray, new Span<byte>(binary.GetNewFile(oldfileArrary, file).ToArray()));
            });
        }

        public void Generate()
        { }
    }
}