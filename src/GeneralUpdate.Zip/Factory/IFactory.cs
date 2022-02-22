using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    public interface IFactory
    {
        IFactory CreateOperation(OperationType type, string sourcePath, string destinationPath, bool includeBaseDirectory = false, Encoding encoding = null);

        IFactory CreateZip();

        IFactory UnZip();
    }
}
