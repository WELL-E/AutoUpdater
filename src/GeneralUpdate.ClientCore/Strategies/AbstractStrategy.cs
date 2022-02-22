using System;
using GeneralUpdate.ClientCore.Update;
using GeneralUpdate.ClientCore.Utils;

namespace GeneralUpdate.ClientCore.Strategies
{
    public abstract class AbstractStrategy : IStrategy
    {
        public virtual void Create(IFile file, Action<object, MutiDownloadProgressChangedEventArgs> eventAction, Action<object, ExceptionEventArgs> errorEventAction)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual T GetOption<T>(UpdateOption<T> option)
        {
            return default(T);
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
        
        protected virtual bool StartApp(string appName) { throw new NotImplementedException(); }
    }
}
