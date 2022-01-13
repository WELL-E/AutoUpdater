using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneralUpdate.Common.CustomAwaiter
{
    public interface ICriticalAwaiter : IAwaiter, ICriticalNotifyCompletion
    {
    }

    public interface ICriticalAwaiter<out TResult> : IAwaiter<TResult>, ICriticalNotifyCompletion
    {
    }
}
