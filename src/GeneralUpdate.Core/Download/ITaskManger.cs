using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Core.Download
{
    internal interface ITaskManger<K,T> where T : class
    {
        /// <summary>
        /// 添加观察者
        /// </summary>
        /// <param name="task"></param>
        void EnPool(K key,T task);

        /// <summary>
        /// 减少观察者
        /// </summary>
        /// <param name="task"></param>
        void DePool(K key);

        /// <summary>
        /// 通知所有下载任务开启下载
        /// </summary>
        void Launch();
    }
}
