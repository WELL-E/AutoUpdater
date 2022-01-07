using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal interface ITask
    {
        bool IsCompleted { get; }

    }
}
