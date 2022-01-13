using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal abstract class AbstractTaskManager<K,T> : ITaskManger<K, T> where T : class
    {
        private ConcurrentDictionary<K, Lazy<T>> _downloadTaskPool;

        internal ConcurrentDictionary<K, Lazy<T>> DownloadTaskPool
        { 
            get => _downloadTaskPool ?? (_downloadTaskPool = new ConcurrentDictionary<K, Lazy<T>>());
        }

        public void DePool(K key)
        {
            DownloadTaskPool.TryRemove(key, out _);
        }

        public void EnPool(K key, T task)
        {
            if (DownloadTaskPool.ContainsKey(key)) return;

            DownloadTaskPool.TryAdd(key, new Lazy<T>(() =>
            {
                return task;
            }));
        }

        public abstract void Launch();
    }
}
