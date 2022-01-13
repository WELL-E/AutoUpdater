using GeneralUpdate.Common.CustomAwaiter;
using GeneralUpdate.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Download
{
    public sealed class DownloadTask<T> : AbstractTask, IAwaiter<T>, IAwaitable<DownloadTask<T>, T> where T : class
    {
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public bool IsCompleted { get; }

        public T GetResult()
        {
            throw new NotImplementedException();
        }

        public DownloadTask<T> GetAwaiter()
        {
            return this;
        }

        public override void Dirty()
        {
            throw new NotImplementedException();
        }
    }
}
