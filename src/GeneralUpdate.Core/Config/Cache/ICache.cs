namespace GeneralUpdate.Core.Config.Cache
{
    public interface ICache<TEntity> where TEntity : class
    {
        void TryAdd(string key, TEntity entity);

        bool TryRemove(string key);

        TEntity TryGet(string key);
    }
}