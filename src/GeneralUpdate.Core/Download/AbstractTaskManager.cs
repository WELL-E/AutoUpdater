using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal abstract class AbstractTaskManager : ITaskManger
    {
        public bool AllComplated()
        {
            throw new NotImplementedException();
        }

        public void EnPool(object task)
        {
            throw new NotImplementedException();
        }

        public void Launch()
        {
            throw new NotImplementedException();
        }


        public void ReslasePool()
        {
            throw new NotImplementedException();
        }
    }
}
