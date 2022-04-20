﻿using System.Threading.Tasks;

namespace GeneralUpdate.Differential.Config.Handles
{
    public interface IHandle<TEntity> where TEntity : class
    {
        /// <summary>
        /// Write the cache content to the file to be updated.
        /// </summary>
        /// <param name="oldEntity"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        Task<bool> Write(TEntity oldEntity, TEntity newEntity);

        /// <summary>
        /// read file content.
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        Task<TEntity> Read(string path);
    }
}