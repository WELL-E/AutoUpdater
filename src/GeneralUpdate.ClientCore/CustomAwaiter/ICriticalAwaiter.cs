using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.ClientCore.CustomAwaiter
{
    public interface ICriticalAwaiter : IAwaiter, ICriticalNotifyCompletion
    {
    }

    public interface ICriticalAwaiter<out TResult> : IAwaiter<TResult>, ICriticalNotifyCompletion
    {
    }
}
