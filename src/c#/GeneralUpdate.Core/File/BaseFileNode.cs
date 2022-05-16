using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.File
{
    internal abstract class BaseFileNode<T> where T : IComparable
    {
        internal int Index;

        internal T[] Keys { get; set; }
        internal int KeyCount;

        internal abstract BaseFileNode<T> GetParent();
        internal abstract BaseFileNode<T>[] GetChildren();

        internal BaseFileNode(int maxKeysPerNode)
        {
            Keys = new T[maxKeysPerNode];
        }

        internal int GetMedianIndex()
        {
            return (KeyCount / 2) + 1;
        }
    }
}
