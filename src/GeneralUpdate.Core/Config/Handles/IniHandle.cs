using GeneralUpdate.Common.CustomAwaiter;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Config.Handles
{
    public class IniHandle<TEntity> : IAwaiter<TEntity> where TEntity : class
    {
        public bool IsCompleted => throw new NotImplementedException();

        public TEntity GetResult()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }
}
