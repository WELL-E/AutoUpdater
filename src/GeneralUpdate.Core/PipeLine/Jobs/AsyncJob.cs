using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine.Jobs
{
    /// <summary>
    /// 
    /// Represents a Job to be processed in the pipeline.
    /// 
    /// </summary>
    /// <typeparam name="TParam">The type of the param.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class AsyncJob<TParam, TResult> : AsyncPipelineJob<TParam, TResult>
    {
        /// <summary>
        /// Performs the job using the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        public override abstract TResult Perform(TParam param);
    }
}
