using GeneralUpdate.Core.Update;
using GeneralUpdate.Core.Utils;
using System;
using System.IO;

namespace GeneralUpdate.Core.Strategys
{
    public class AbstractStrategy
    {
        public virtual T GetOption<T>(UpdateOption<T> option)
        {
            return default;
        }

        public bool VerifyFileMd5(string fileName, string md5)
        {
            var packetMD5 = FileUtil.GetFileMD5(fileName);

            if (md5.ToUpper().Equals(packetMD5.ToUpper()))
            {
                return true;
            }
            return false;
        }
    }
}
