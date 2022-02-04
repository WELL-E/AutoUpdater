using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using GeneralUpdate.Zip.GZip.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

/*
 * Source address : https://www.cnblogs.com/Chary/p/No0000DF.html
 * author ：CharyGao
 */
namespace GeneralUpdate.Zip.GZip
{
    public class GeneralZip : IOperation
    {
        private string _sourcePath, _targetPath;

        public delegate void UnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
        public event UnZipProgressEventHandler UnZipProgress;

        public delegate void CompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);
        public event CompressProgressEventHandler CompressProgress;

        /// <summary>
        /// Creates a zip archive containing the files and subdirectories of the specified directory. 
        /// </summary>
        /// <param name="sourceDirectoryName">The path of the file directory to be compressed and archived, which can be a relative path or an absolute path. A relative path is a path relative to the current working directory. </param>
        /// <param name="destinationArchiveFileName">The archive path of the compressed package to be generated, which can be a relative path or an absolute path. A relative path is a path relative to the current working directory. </param>
        /// <param name="compressionLevel">Enumeration value indicating whether the compression operation emphasizes speed or compression size .</param>
        /// <param name="includeBaseDirectory">Whether the archive contains the parent directory .</param>
        public bool CreatZip(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.NoCompression, bool includeBaseDirectory = true)
        {
            int i = 1;
            try
            {
                if (Directory.Exists(sourceDirectoryName))
                    if (!File.Exists(destinationArchiveFileName))
                    {
                        ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
                    }
                    else
                    {
                        var toZipFileDictionaryList = GetAllDirList(sourceDirectoryName, includeBaseDirectory);
                        using (var archive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Update))
                        {
                            var count = toZipFileDictionaryList.Keys.Count;
                            foreach (var toZipFileKey in toZipFileDictionaryList.Keys)
                            {
                                if (toZipFileKey != destinationArchiveFileName)
                                {
                                    var toZipedFileName = Path.GetFileName(toZipFileKey);
                                    var toDelArchives = new List<ZipArchiveEntry>();
                                    foreach (var zipArchiveEntry in archive.Entries)
                                    {
                                        if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                        {
                                            i++;
                                            CompressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = zipArchiveEntry.FullName , Name = zipArchiveEntry.Name });
                                            toDelArchives.Add(zipArchiveEntry);
                                        }
                                    }

                                    foreach (var zipArchiveEntry in toDelArchives)
                                        zipArchiveEntry.Delete();
                                    archive.CreateEntryFromFile(toZipFileKey, toZipFileDictionaryList[toZipFileKey], compressionLevel);
                                }
                            }
                        }
                    }
                else if (File.Exists(sourceDirectoryName))
                    if (!File.Exists(destinationArchiveFileName))
                        ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, false);
                    else
                    {
                        using (var archive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Update))
                        {
                            if (sourceDirectoryName != destinationArchiveFileName)
                            {
                                var toZipedFileName = Path.GetFileName(sourceDirectoryName);
                                var toDelArchives = new List<ZipArchiveEntry>();
                                var count = archive.Entries.Count;
                                foreach (var zipArchiveEntry in archive.Entries)
                                {
                                    if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                    {
                                        i++;
                                        CompressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = zipArchiveEntry.FullName, Name = zipArchiveEntry.Name });
                                        toDelArchives.Add(zipArchiveEntry);
                                    }
                                }

                                foreach (var zipArchiveEntry in toDelArchives)
                                    zipArchiveEntry.Delete();
                                archive.CreateEntryFromFile(sourceDirectoryName, toZipedFileName, compressionLevel);
                            }
                        }
                    }
                else
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a zip archive containing the files and directories of the specified directory. 
        /// </summary>
        /// <param name="sourceDirectoryName">The path of the file directory to be compressed and archived, which can be a relative path or an absolute path. A relative path is a path relative to the current working directory.</param>
        /// <param name="destinationArchiveFileName">The archive path of the compressed package to be generated, which can be a relative path or an absolute path. A relative path is a path relative to the current working directory.</param>
        /// <param name="compressionLevel">Enumeration value indicating whether the compression operation emphasizes speed or compression size.</param>
        public bool CreatZip(Dictionary<string, string> sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            int i = 1;
            try
            {
                using (FileStream zipToOpen = new FileStream(destinationArchiveFileName, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        foreach (var toZipFileKey in sourceDirectoryName.Keys)
                        {
                            if (toZipFileKey != destinationArchiveFileName)
                            {
                                var toZipedFileName = Path.GetFileName(toZipFileKey);
                                var toDelArchives = new List<ZipArchiveEntry>();
                                var count = archive.Entries.Count;
                                foreach (var zipArchiveEntry in archive.Entries)
                                {
                                    if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                    {
                                        i++;
                                        CompressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = toZipedFileName });
                                        toDelArchives.Add(zipArchiveEntry);
                                    }
                                }

                                foreach (var zipArchiveEntry in toDelArchives)
                                    zipArchiveEntry.Delete();
                                archive.CreateEntryFromFile(toZipFileKey, sourceDirectoryName[toZipFileKey], compressionLevel);
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Recursively delete the specified folder directory and file on the disk.
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <returns></returns>
        private bool DeleteFolder(string baseDirectory)
        {
            var successed = true;
            try
            {
                if (Directory.Exists(baseDirectory)) //如果存在这个文件夹删除之 
                {
                    foreach (var directory in Directory.GetFileSystemEntries(baseDirectory))
                        if (File.Exists(directory))
                            File.Delete(directory); //直接删除其中的文件  
                        else
                            successed = DeleteFolder(directory); //递归删除子文件夹 
                    Directory.Delete(baseDirectory); //删除已空文件夹     
                }
            }
            catch
            {
                successed = false;
            }
            return successed;
        }

        /// <summary>
        /// Recursively get the set of all files in the specified directory on the disk, the return type is: dictionary [file name, relative file name to be compressed] 
        /// </summary>
        /// <param name="strBaseDir"></param>
        /// <param name="includeBaseDirectory"></param>
        /// <param name="namePrefix"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetAllDirList(string strBaseDir, bool includeBaseDirectory = false, string namePrefix = "")
        {
            var resultDictionary = new Dictionary<string, string>();
            var directoryInfo = new DirectoryInfo(strBaseDir);
            var directories = directoryInfo.GetDirectories();
            var fileInfos = directoryInfo.GetFiles();
            if (includeBaseDirectory)
                namePrefix += directoryInfo.Name + "\\";
            foreach (var directory in directories)
                resultDictionary = resultDictionary.Concat(GetAllDirList(directory.FullName, true, namePrefix)).ToDictionary(k => k.Key, k => k.Value); //FullName是某个子目录的绝对地址，
            foreach (var fileInfo in fileInfos)
                if (!resultDictionary.ContainsKey(fileInfo.FullName))
                    resultDictionary.Add(fileInfo.FullName, namePrefix + fileInfo.Name);
            return resultDictionary;
        }

        /// <summary>
        /// Unzip the Zip file and save it to the specified target path folder .
        /// </summary>
        /// <param name="zipFilePath">将要解压缩的zip文件的路径</param>
        /// <param name="unZipDir">解压后将zip中的文件存储到磁盘的目标路径</param>
        /// <returns></returns>
        public bool UnZip(string zipFilePath, string unZipDir)
        {
            bool resualt;
            try
            {
                unZipDir = unZipDir.EndsWith(@"\") ? unZipDir : unZipDir + @"\";
                var directoryInfo = new DirectoryInfo(unZipDir);
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
                var fileInfo = new FileInfo(zipFilePath);
                if (!fileInfo.Exists)
                    return false;
                using (var zipToOpen = new FileStream(zipFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        var count = archive.Entries.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var entries = archive.Entries[i];
                            if (!entries.FullName.EndsWith("/"))
                            {
                                var entryFilePath = Regex.Replace(entries.FullName.Replace("/", @"\"), @"^\\*", "");
                                var filePath = directoryInfo + entryFilePath;
                                UnZipProgress(this, new BaseUnZipProgressEventArgs { Size = entries.Length, Count = count, Index = i + 1 , Path = entries.FullName, Name = entries.Name }); 
                                 var content = new byte[entries.Length];
                                entries.Open().Read(content, 0, content.Length);
                                var greatFolder = Directory.GetParent(filePath);
                                if (!greatFolder.Exists)
                                    greatFolder.Create();
                                File.WriteAllBytes(filePath, content);
                            }
                        }
                    }
                }
                resualt = true;
            }
            catch
            {
                resualt = false;
            }
            return resualt;
        }

        /// <summary>
        /// Get a list of files in a Zip archive .
        /// </summary>
        /// <param name="zipFilePath">The physical path of the Zip archive file.</param>
        /// <returns></returns>
        public List<string> GetZipFileList(string zipFilePath)
        {
            List<string> fList = new List<string>();
            if (!File.Exists(zipFilePath))
                return fList;
            try
            {
                using (var zipToOpen = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        foreach (var zipArchiveEntry in archive.Entries)
                            if (!zipArchiveEntry.FullName.EndsWith("/"))
                                fList.Add(Regex.Replace(zipArchiveEntry.FullName.Replace("/", @"\"), @"^\\*", ""));
                    }
                }
            }
            catch
            {
                return null;
            }
            return fList;
        }

        public bool CreatZip()
        {
            return CreatZip(_sourcePath, _targetPath);
        }

        public bool UnZip()
        {
            return UnZip(_sourcePath,_targetPath);
        }

        public void OnCompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e)
        {
            if (CompressProgress != null) CompressProgress(sender,e);
        }

        public void OnUnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e)
        {
            if(UnZipProgress != null) UnZipProgress(sender,e);
        }

        public void Configs(string sourcePath, string targetPath)
        {
            _sourcePath = sourcePath;
            _targetPath = targetPath;
        }
    }
}
