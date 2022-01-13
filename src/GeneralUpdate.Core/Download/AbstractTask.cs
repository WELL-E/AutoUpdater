using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    public abstract class AbstractTask : ITask//, IProgress<T> 
    {
        //public void Report(T value)
        //{
        //    throw new NotImplementedException();
        //}
        public abstract void Dirty();
    }
}
