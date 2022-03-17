using GeneralUpdate.Core.PipeLine.Stages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace GeneralUpdate.Core.PipeLine
{
    public static class Pipeline
    {
        /// <summary>
        /// Creates a new Pipeline with a Cancellation token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static IPipeline<TParam> Create<TParam>(CancellationToken token)
        {
            return null;
        }

        /// <summary>
        /// Creates a new Pipeline.
        /// </summary>
        /// <returns></returns>
        public static IPipeline<TParam> Create<TParam>(StageSetup<double, string> stageSetup)
        {
            return new BasicPipeline<TParam>(stageSetup.ToEnumerable());
        }
    }
}
