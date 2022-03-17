using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine.Collections
{
    public class Queue<T> : IQueue<T>
    {
        private readonly LinkedList<T> collection = new LinkedList<T>();

        public void Add(T element)
        {
            collection.AddLast(element);
        }

        public IEnumerable<T> GetElements()
        {
            var element = collection.First;
            while (element != null)
            {
                collection.RemoveFirst();
                yield return element.Value;
                element = collection.First;
            }
        }

        public int Count
        {
            get { return collection.Count; }
        }


        public void CompleteAdding() { }
    }
}
