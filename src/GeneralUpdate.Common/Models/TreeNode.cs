using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Common.Models
{
    public class TreeNode
    {
        public string Name { get; set; }

        public byte[] FileBytes { get; set; }

        public string MD5 { get; set; }

        public string Path { get; set; }

        public TreeNode LeftNode { get; set; }

        public TreeNode RightNode { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
