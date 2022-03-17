using GeneralUpdate.Core.PipeLine.Collections;
using GeneralUpdate.Core.PipeLine.Jobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GeneralUpdate.Core.PipeLine.Stages
{
    internal static class StageSetupex 
    {
        public static IEnumerable<IPipelineJob> ToEnumerable<TParam, TResult>(this StageSetup<TParam, TResult> stageSetup)
        {
            var current = stageSetup.Current.First;

            while (current != null)
            {
                yield return current.Job;
                current = current.Next;
            }
        }

        public static IQueue<Object> ToQueue<T>(this IEnumerable<T> source)
        {
            var queue = new BlockingQueue<Object>();

            foreach (var item in source)
            {
                queue.Add(item);
            }

            queue.CompleteAdding();

            return queue;
        }
    }

    public class StageSetup<TParam, TResult>
    {
        // Current stage.
        private IStage stage;

        /// <summary>
        /// Gets the current stage.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        internal IStage Current { get { return stage; } }

        internal StageSetup(IStage stage)
        {
            this.stage = stage;

            // If the first is not set then this must be the first one.
            // Make it point to me.
            if (stage.First == null)
            {
                stage.First = stage;
            }
        }

        public StageSetup<TResult, TNextResult> Stage<TNextResult>(AsyncJob<TResult, TNextResult> job)
        {
            // Append the next stage
            stage.Next = new Stage(job)
            {
                First = stage.First
            };

            // Wrap the new stage with a setup
            return new StageSetup<TResult, TNextResult>(stage.Next);
        }

        public StageSetup<TResult, TNextResult> Stage<TNextResult>(Func<TResult, TNextResult> func)
        {
            return Stage(new LambdaAsyncJob<TResult, TNextResult>(func));
        }
    }
}
