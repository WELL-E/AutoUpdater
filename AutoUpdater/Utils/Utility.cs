using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace AutoUpdater.Utils
{
    internal static class Utility
    {
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
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, isOverWrite);
                if (action != null)
                {
                    action.Invoke(file.Name);
                }
            }

            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, true, isOverWrite, action);
                }
            }
        }

        internal static string GetTempDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        internal static void UpdateReg(RegistryKey baseKey, string subKey, string keyName, string keyValue)
        {
            using (var registry = new RegistryHepler(baseKey, subKey))
            {
                if (!String.IsNullOrEmpty(registry.Read(keyName)))
                {
                    registry.Write(keyName, keyValue);
                }
            }
        }

        internal static string ConvertToUnit(long byteSize)
        {
            string str;
            var tempSize = Convert.ToSingle(byteSize);
            if (tempSize / 1024 > 1)
            {
                if ((tempSize / 1024) / 1024 > 1)
                {
                    str = ((tempSize / 1024) / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "MB/S";
                }
                else
                {
                    str = (tempSize / 1024).ToString("##0.00", CultureInfo.InvariantCulture) + "KB/S";
                }
            }
            else
            {
                str = tempSize.ToString(CultureInfo.InvariantCulture) + "B/S";
            }

            return str;
        }

    }
}
