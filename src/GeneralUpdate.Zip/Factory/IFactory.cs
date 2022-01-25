using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    public interface IFactory
    {
        IFactory CreatefOperate(OperationType type);

        IFactory CreatZip();

        IFactory UnZip();

        IFactory Configs(string sourcePath,string targetPath);
    }
}
