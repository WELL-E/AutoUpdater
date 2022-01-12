using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal abstract class AbstractTask<T> : INotifyCompletion , IProgress<T> where T : class
    {
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public void Report(T value)
        {
            throw new NotImplementedException();
        }
    }
}
