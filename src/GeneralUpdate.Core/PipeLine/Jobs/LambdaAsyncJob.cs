using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine.Jobs
{
    public class LambdaAsyncJob<TParam, TResult> : AsyncJob<TParam, TResult>
    {
        private Func<TParam, TResult> func;

        public LambdaAsyncJob(Func<TParam, TResult> func)
        {
            this.func = func;
        }

        public override TResult Perform(TParam param)
        {
            return func(param);
        }
    }
}
