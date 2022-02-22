using GeneralUpdate.ClientCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GeneralUpdate.ClientCore.Utils
{
    internal static class FileUtil
    {
        public static bool CreateFolder(string path) 
        {
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    DeleteDir(path);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool CopyFiles(List<string> pathList,string targetPath) 
        {
            foreach (var path in pathList)
            {
                bool isDone = CopyFile(path, targetPath);
                if (!isDone)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CopyFile(string filePath, string targetPath) 
        {
            try
            {
                File.Copy(filePath, targetPath, true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool DeleteFile(string path) 
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void DeleteDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);
                    }
                    else
                    {
                        File.Delete(i.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static List<FileBase> GetFiles(string packetPath)
        {
            if (!Directory.Exists(packetPath)) return null;

            List<FileBase> lstFile = new List<FileBase>();
            string[] files = Directory.GetFiles(packetPath);
            foreach (var file in files)
            {
                var fileBase = new FileBase();
                fileBase.MD5 = GetFileMD5(file);
                fileBase.Name = Path.GetFileName(file);
                fileBase.Path = file;
                lstFile.Add(fileBase);
            }
            return lstFile;
        }

        public static bool VerifyFileMd5(string fileName, string md5)
        {
            var packetMD5 = GetFileMD5(fileName);

            if (!md5.ToUpper().Equals(packetMD5.ToUpper()))
            {
                Directory.Delete(fileName, true); ;
                return false;
            }
            return true;
        }

        public static bool UnPacket(string filePath, string tempPath)
        {
            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(filePath, tempPath);
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Update32Or64Libs(string currentDir)
        {
            var is64XSystem = Environment.Is64BitOperatingSystem;
            var sourceDir = Path.Combine(currentDir, is64XSystem ? "x64" : "x32");
            var destDir = Path.Combine(currentDir, "dlls");

            if (!Directory.Exists(sourceDir)) return;
            FileUtil.DirectoryCopy(sourceDir, destDir, true, true, null);
            Directory.Delete(sourceDir);
        }

        public static JObject ConfigurationBuilder(string jsonPath)
        {
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonPath))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    return o;
                }
            }
        }

        internal static string GetFileMD5(string fileName)
        {
            try
            {
                var file = new FileStream(fileName, FileMode.Open);
                var md5 = new MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        internal static void DirectoryCopy(string sourceDirName, string destDirName,
          bool copySubDirs, bool isOverWrite, Action<string> action)
        {
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                return;
            }

            var dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, isOverWrite);
                if (action != null)
                {
                    action.Invoke(file.Name);
                }
            }

            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, true, isOverWrite, action);
                }
            }

            Directory.Delete(sourceDirName, true);
        }

        internal static string GetTempDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        internal static string GetTempDirectory(string version)
        {
            var path2 = $"generalupdate_{ DateTime.Now.ToString("yyyy-MM-dd") }_{version}";
            var tempDir = Path.Combine(Path.GetTempPath(), path2);
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            return tempDir;
        }

        internal static bool InitConfig(string path, dynamic obj)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        string json = JsonConvert.SerializeObject(obj);
                        streamWriter.Write(json);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static T ReadConfig<T>(string path) 
        {
            try
            {
                if (File.Exists(path)) 
                {
                    using (StreamReader streamReader = File.OpenText(path))
                    {
                        string json = streamReader.ReadToEnd();
                        return JsonConvert.DeserializeObject<T>(json);
                    }
                }
            }
            catch (Exception)
            {
            }
            return default(T);
        }
    }
}
