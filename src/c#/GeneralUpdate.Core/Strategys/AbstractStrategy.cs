using GeneralUpdate.Core.Models;
using GeneralUpdate.Core.Update;
using System;

namespace GeneralUpdate.Core.Strategys
{
    public abstract class AbstractStrategy : IStrategy
    {
        public virtual void Create(string platformType,IFile file, Action<object, MutiDownloadProgressChangedEventArgs> eventAction, Action<object, ExceptionEventArgs> errorEventAction) =>
            throw new NotImplementedException();

        public virtual void Excute() => throw new NotImplementedException();

        protected virtual bool StartApp(string appName) => throw new NotImplementedException();
    }
}