using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.File
{
    internal class FileNode<T> : BaseFileNode<T> where T : IComparable
    {
        internal FileNode<T> Parent { get; set; }
        internal FileNode<T>[] Children { get; set; }

        internal bool IsLeaf => Children[0] == null;

        internal FileNode(int maxKeysPerNode, FileNode<T> parent)
            : base(maxKeysPerNode)
        {
            Parent = parent;
            Children = new FileNode<T>[maxKeysPerNode + 1];
        }

        internal override BaseFileNode<T> GetParent()
        {
            return Parent;
        }

        internal override BaseFileNode<T>[] GetChildren()
        {
            return Children;
        }

        public FileNode<T> Prev { get; set; }

        public FileNode<T> Next { get; set; }
    }
}
