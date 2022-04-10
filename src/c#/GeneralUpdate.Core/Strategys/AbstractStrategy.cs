using GeneralUpdate.Core.Update;
using System;

namespace GeneralUpdate.Core.Strategys
{
    public abstract class AbstractStrategy : IStrategy
    {
        public virtual void Create(IFile file, Action<object, MutiDownloadProgressChangedEventArgs> eventAction, Action<object, ExceptionEventArgs> errorEventAction) =>
            throw new NotImplementedException();

        public virtual void Excute() => throw new NotImplementedException();

        public virtual T GetOption<T>(UpdateOption<T> option) => throw new NotImplementedException();

        protected virtual bool StartApp(string appName) => throw new NotImplementedException();
    }
}