using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal abstract class AbstractTask<T> : IProgress<T> where T : class
    {
        public void Report(T value)
        {
            throw new NotImplementedException();
        }
    }
}
