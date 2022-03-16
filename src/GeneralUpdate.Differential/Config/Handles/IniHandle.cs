using GeneralUpdate.Common.CustomAwaiter;
using System;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential.Config.Handles
{
    public class IniHandle<TEntity> : IHandle<TEntity>, IAwaiter<TEntity> where TEntity : class
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

        public Task<TEntity> Read(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Write(TEntity oldEntity, TEntity newEntity)
        {
            throw new NotImplementedException();
        }
    }
}