using GeneralUpdate.Differential.BinaryFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential
{
    public class DifferentialCore
    {
        private async Task<bool> Comparebytes(byte[] oldfileArrary, byte[] newfileArray)
        {
            return await Task.Run(() =>
            {
                Binary binary = new Binary();
                List<IBinaryFile> file = binary.GetBinary(oldfileArrary, newfileArray);
                file = binary.Deserialize(binary.Serialize(file));
                return binary.Equals(newfileArray, new Span<byte>(binary.GetNewFile(oldfileArrary, file).ToArray()));
            });
        }

        /// <summary>
        /// Generate diff file.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public async Task Generate(string sourcePath,string targetPath)
        {
            List<string> sourceFiles = new List<string>();
            List<string> targetFiles = new List<string>();
            Find(sourcePath, ref sourceFiles);
            Find(targetPath, ref targetFiles);
            foreach (var item in sourceFiles)
            {
                var sourceFileBytes = File.ReadAllBytes(item);
                var targetFileBytes = File.ReadAllBytes(item);
                await Comparebytes(sourceFileBytes, targetFileBytes);
            }
        }

        /// <summary>
        /// Find matching files recursively.
        /// </summary>
        /// <param name="directory">root directory</param>
        /// <param name="files">result file list</param>
        private void Find(string rootDirectory, ref List<string> files)
        {
            var rootDirectoryInfo = new DirectoryInfo(rootDirectory);
            foreach (var file in rootDirectoryInfo.GetFiles())
            {
                var fullName = file.FullName;
                files.Add(fullName);
            }
            foreach (var dir in rootDirectoryInfo.GetDirectories())
            {
                Find(dir.FullName, ref files);
            }
        }
    }
}