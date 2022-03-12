using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GeneralUpdate.Common.Utils
{
    public static class FileUtil
    {
        public static void Update32Or64Libs(string currentDir)
        {
            var is64XSystem = Environment.Is64BitOperatingSystem;
            var sourceDir = Path.Combine(currentDir, is64XSystem ? "x64" : "x32");
            var destDir = Path.Combine(currentDir, "dlls");

            if (!Directory.Exists(sourceDir)) return;
            DirectoryCopy(sourceDir, destDir, true, true, null);
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

        /// <summary>
        /// Compare the contents of two folders for equality.
        /// </summary>
        /// <param name="sourcePath">source path</param>
        /// <param name="targetPath">target path</param>
        /// <returns>result1 :  The following files are in both folders, result2 : The following files are in list1 but not list2.</returns>
        public static (IEnumerable<FileInfo>, IEnumerable<FileInfo>) Compare(string sourcePath,string targetPath) 
        {
            // Create two identical or different temporary folders
            // on a local drive and change these file paths.
            var dirSource = new DirectoryInfo(sourcePath);
            var dirTarget = new DirectoryInfo(targetPath);

            // Take a snapshot of the file system.  
            IEnumerable<FileInfo> listSource = dirSource.GetFiles("*.*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> listTarget = dirTarget.GetFiles("*.*", SearchOption.AllDirectories);

            //A custom file comparer defined below  
            var fileCompare = new FileCompare();

            // This query determines whether the two folders contain  
            // identical file lists, based on the custom file comparer  
            // that is defined in the FileCompare class.  
            // The query executes immediately because it returns a bool. 
            //areIdentical  true : the two folders are the same; false : The two folders are not the same.
            bool areIdentical = listSource.SequenceEqual(listTarget, fileCompare);

            // Find the common files. It produces a sequence and doesn't
            // execute until the foreach statement.  
            var queryCommonFiles = listSource.Intersect(listTarget, fileCompare);

            if (queryCommonFiles.Any())
            {
                Console.WriteLine("The following files are in both folders:");
                foreach (var v in queryCommonFiles)
                {
                    //shows which items end up in result list  
                    Console.WriteLine(v.FullName); 
                }
            }
            else
            {
                Console.WriteLine("There are no common files in the two folders.");
            }

            // Find the set difference between the two folders.  
            // For this example we only check one way.  
            var queryList1Only = (from file in listSource
                                  select file).Except(listTarget, fileCompare);

            Console.WriteLine("The following files are in list1 but not list2:");
            foreach (var v in queryList1Only)
            {
                Console.WriteLine(v.FullName);
            }

            return (queryCommonFiles, queryList1Only);
        }
    }

    /// <summary>
    /// This implementation defines a very simple comparison  
    /// between two FileInfo objects. It only compares the name  
    /// of the files being compared and their length in bytes.  
    /// </summary>
    public class FileCompare : IEqualityComparer<FileInfo> 
    {
        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return (f1.Name == f2.Name &&
                    f1.Length == f2.Length);
        }

        // Return a hash that reflects the comparison criteria. According to the
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}";
            return s.GetHashCode();
        }
    }
}