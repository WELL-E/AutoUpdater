using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GeneralUpdate.Core.PipeLine.Collections
{
    public class BlockingQueue<T> : IQueue<T>
    {
        private readonly BlockingCollection<T> collection = new BlockingCollection<T>();

        public void Add(T element)
        {
            collection.Add(element);
        }

        public IEnumerable<T> GetElements()
        {
            return collection.GetConsumingEnumerable();
        }

        public int Count
        {
            get { return collection.Count; }
        }


        public void CompleteAdding()
        {
            collection.CompleteAdding();
        }
    }
}
