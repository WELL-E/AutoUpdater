using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal interface ITaskManger
    {
        bool AllComplated();

        void EnPool(object task);

        void ReslasePool();

        void Launch();


    }
}
