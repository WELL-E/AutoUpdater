using GeneralUpdate.Common.Models;
using GeneralUpdate.Common.Utils;
using GeneralUpdate.Differential.BinaryFile;
using GeneralUpdate.Zip;
using GeneralUpdate.Zip.Events;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential
{
    public class DifferentialCore
    {
        private Action<object, BaseCompressProgressEventArgs> _compressProgressCallback;

        private bool Comparebytes(byte[] oldfileArrary, byte[] newfileArray)
        {
            var binary = new Binary();
            var file = binary.GetBinary(oldfileArrary, newfileArray);
            file = binary.Deserialize(binary.Serialize(file));
            return binary.Equals(newfileArray, new Span<byte>(binary.GetNewFile(oldfileArrary, file).ToArray()));
        }

        /// <summary>
        /// Generate diff file.
        /// </summary>
        /// <param name="sourcePath">Previous version folder path .</param>
        /// <param name="targetPath">Recent version folder path.</param>
        /// <param name="compressProgressCallback">Incremental package generation progress callback function.</param>
        /// <param name="diffPath">Store discovered incremental update files in a temporary directory .</param>
        /// <param name="generatePath">Incremental package generation path .</param>
        /// <param name="type">7z or zip</param>
        /// <param name="encoding">Incremental packet encoding format .</param>
        /// <returns></returns>
        public async Task Generate(string sourcePath,string targetPath, Action<object , BaseCompressProgressEventArgs> compressProgressCallback, string diffPath = null, string generatePath = null, OperationType type = OperationType.GZip, Encoding encoding = null)
        {
            await Task.Run(() => 
            {
                if(string.IsNullOrWhiteSpace(diffPath))diffPath = Environment.CurrentDirectory;
                if (string.IsNullOrWhiteSpace(generatePath)) generatePath = Environment.CurrentDirectory;
                _compressProgressCallback = compressProgressCallback;
                var tupleResult = FileUtil.Compare(sourcePath, targetPath);
                foreach (var file in tupleResult.Item2)
                {
                    var findFile = tupleResult.Item1.FirstOrDefault(f => f.Name.Equals(file.Name));
                    if (findFile == null) continue;
                    Comparebytes(File.ReadAllBytes(findFile.FullName), File.ReadAllBytes(file.FullName));
                }
                var factory = new GeneralZipFactory();
                factory.CompressProgress += OnCompressProgress;
                factory.CreatefOperate(type, diffPath, generatePath,false,encoding).CreatZip();
            });
        }

        private void OnCompressProgress(object sender, BaseCompressProgressEventArgs e) => _compressProgressCallback(sender,e);
    }
}