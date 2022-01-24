using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    internal interface IOperation
    {
        bool CreatZip();

        bool Zip();
    }
}
