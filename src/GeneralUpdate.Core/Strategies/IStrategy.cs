using System;
using GeneralUpdate.Core.Update;

namespace GeneralUpdate.Core.Strategies
{
    /// <summary>
    /// Update the strategy, if you need to extend it, you need to inherit this interface.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// execution strategy.
        /// </summary>
        void Execute();

        /// <summary>
        /// Create a policy.
        /// </summary>
        /// <param name="file">Abstraction for updating package information.</param>
        void Create(IFile file, Action<object, MultiDownloadProgressChangedEventArgs> eventAction,Action<object, ExceptionEventArgs> errorEventAction);
    }
}
