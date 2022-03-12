using GeneralUpdate.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneralUpdate.Common.Utils
{
    public class TreeUtil
    {
        /// <summary>
        /// Find matching files recursively.
        /// </summary>
        /// <param name="directory">root directory</param>
        /// <param name="files">result file list</param>
        public static void Find(string rootDirectory, ref List<string> files)
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

        public static bool IsSameTree(TreeNode p, TreeNode q)
        {
            if (p == null && q == null) return true;
            if (p == null && q != null) return false;
            if (p != null && q == null) return false;
            if (p.MD5 != q.MD5)
                return false;
            else
                return IsSameTree(p.RightNode, q.LeftNode) && IsSameTree(p.RightNode, q.LeftNode);
        }
    }
}
