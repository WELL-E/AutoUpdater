using GeneralUpdate.Core.PipeLine.Collections;
using System;

namespace GeneralUpdate.Core.PipeLine
{
    interface IPipelineJob<TParam, TResult>
    {
        TResult InternalPerform(TParam param);
        IQueue<TResult> Output { get; }
        bool PerformAsync { get; }
    }

    interface IPipelineJob : IPipelineJob<object, object>
    {
    }
}