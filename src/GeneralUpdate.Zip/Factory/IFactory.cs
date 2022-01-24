using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    internal interface IFactory
    {
        IFactory CreatefOperate(OperationType type);

        IFactory CreatZip();

        IFactory Zip();

        IFactory Configs(string sourcePath,string targetPath);
    }
}
