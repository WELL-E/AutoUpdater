using System.Collections.Generic;

namespace GeneralUpdate.Core.Download
{
    /// <summary>
    /// Abstract task manager class.
    /// </summary>
    /// <typeparam name="T">'T' is the download task.</typeparam>
    public abstract class AbstractTaskManager<T> : ITaskManger<ITask<T>>
    {
        public abstract void Remove(ITask<T> task);

        public abstract void Add(ITask<T> task);
    }
}