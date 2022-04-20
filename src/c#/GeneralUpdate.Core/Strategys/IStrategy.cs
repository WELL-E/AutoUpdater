﻿using GeneralUpdate.Core.Update;
using System;

namespace GeneralUpdate.Core.Strategys
{
    /// <summary>
    /// Update the strategy, if you need to extend it, you need to inherit this interface.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// execution strategy.
        /// </summary>
        void Excute();

        /// <summary>
        /// Create a policy.
        /// </summary>
        /// <param name="file">Abstraction for updating package information.</param>
        void Create(IFile file, Action<object, MutiDownloadProgressChangedEventArgs> eventAction, Action<object, ExceptionEventArgs> errorEventAction);
    }
}