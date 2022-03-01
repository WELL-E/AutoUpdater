using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace GeneralUpdate.Core.Config.Cache
{
    public class ConfigCache<TEntity> : ICache<TEntity> where TEntity : class
    {
        //TODO: 封装树形结构扫描（机制）复用到增量更新
        private ImmutableDictionary<string, TEntity> _cache = null;
        private ImmutableDictionary<string, TEntity>.Builder _cacheBuilder = null;

        public void TryAdd(string key, TEntity entity) 
        {
            if (!_cacheBuilder.ContainsKey(key)) _cacheBuilder.Add(key, entity);
        }

        public TEntity TryGet(string key)
        {
            TEntity result = null;
            if (_cacheBuilder.ContainsKey(key)) _cacheBuilder.TryGetValue(key, out result);
            return result;
        }

        public bool TryRemove(string key)
        {
            bool isRemove = false;
            if (_cacheBuilder.ContainsKey(key)) isRemove = _cacheBuilder.Remove(key);
            return isRemove;
        }
    }
}
