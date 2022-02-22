using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.ClientCore.ZipFactory.Factory
{
    public interface IFactory
    {
        IFactory CreatefOperate(OperationType type, string sourcePath, string destinationPath, bool includeBaseDirectory = false, Encoding encoding = null);

        IFactory CreatZip();

        IFactory UnZip();
    }
}
