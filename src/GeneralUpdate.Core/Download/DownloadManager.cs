using GeneralUpdate.Common.CustomAwaiter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Download
{
    internal sealed class DownloadManager<T> : AbstractTaskManager, IAwaiter<T>, IAwaitable<DownloadManager<T>, T> where T : class
    {
        public DownloadManager() { }

        public DownloadManager(int maxSize) 
        {
            
        }

        public void OnCompleted(Action continuation)
        {
            continuation.Invoke();
        }

        public bool IsCompleted { get; }

        public T GetResult()
        {
            throw new NotImplementedException();
        }

        public DownloadManager<T> GetAwaiter()
        {
            return this;
        }
    }
}
