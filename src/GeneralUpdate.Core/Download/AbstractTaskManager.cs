using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal class AbstractTaskManager : INotifyCompletion
    {
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }
}
