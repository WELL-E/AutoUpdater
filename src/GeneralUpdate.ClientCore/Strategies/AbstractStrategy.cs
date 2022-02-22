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

        protected virtual bool StartApp(string appName) { throw new NotImplementedException(); }
    }
}
