using GeneralUpdate.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GeneralUpdate.Core.Utils
{
    public static class FileUtil
    {
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

        public static void Update32Or64Libs(string currentDir)
        {
            var is64XSystem = Environment.Is64BitOperatingSystem;
            var sourceDir = Path.Combine(currentDir, is64XSystem ? "x64" : "x32");
            var destDir = Path.Combine(currentDir, "dlls");

            if (!Directory.Exists(sourceDir)) return;
            FileUtil.DirectoryCopy(sourceDir, destDir, true, true, null);
            Directory.Delete(sourceDir);
        }

        public static string GetFileMD5(string fileName)
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

        public static void DirectoryCopy(string sourceDirName, string destDirName,
          bool copySubDirs, bool isOverWrite, Action<string> action)
        {
            var dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists) return;
            var dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            foreach (var file in dir.GetFiles())
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, isOverWrite);
                if (action != null) action.Invoke(file.Name);
            }
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, true, isOverWrite, action);
                }
            }
            Directory.Delete(sourceDirName, true);
        }

        public static string GetTempDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        public static string GetTempDirectory(string version)
        {
            var path2 = $"generalupdate_{ DateTime.Now.ToString("yyyy-MM-dd") }_{version}";
            var tempDir = Path.Combine(Path.GetTempPath(), path2);
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            return tempDir;
        }
    }
}