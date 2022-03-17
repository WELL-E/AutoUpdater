using GeneralUpdate.Core.PipeLine.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine.Jobs
{
    public abstract class AsyncPipelineJob<TParam, TResult> : IPipelineJob
    {
        public AsyncPipelineJob()
        {
            Output = new BlockingQueue<object>();
        }

        /// <summary>
        /// Performs the job using the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        public abstract TResult Perform(TParam param);

        public object InternalPerform(object param)
        {
            var result = Perform((TParam)param);
            Output.Add(result);
            return result;
        }

        public IQueue<object> Output { get; private set; }

        public bool PerformAsync { get { return true; } }
    }
}
