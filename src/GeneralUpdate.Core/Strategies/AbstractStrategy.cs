using System;
using GeneralUpdate.Core.Update;

namespace GeneralUpdate.Core.Strategies
{
    public abstract class AbstractStrategy : IStrategy
    {
        public virtual void Create(IFile file, Action<object, MultiDownloadProgressChangedEventArgs> eventAction, Action<object, ExceptionEventArgs> errorEventAction)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual T GetOption<T>(UpdateOption<T> option)
        {
            throw new NotImplementedException();
        }

        protected virtual bool StartApp(string appName) { throw new NotImplementedException(); }
    }
}
