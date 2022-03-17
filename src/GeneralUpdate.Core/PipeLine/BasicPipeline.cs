using GeneralUpdate.Core.PipeLine.Collections;
using GeneralUpdate.Core.PipeLine.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.PipeLine
{
    class BasicPipeline<TParam> : IPipeline<TParam>
    {
        private IEnumerable<IPipelineJob> jobs;

        internal BasicPipeline(IEnumerable<IPipelineJob> jobs)
        {
            this.jobs = jobs;
        }

        public void Run(IEnumerable<TParam> param)
        {
            var factory = Task.Factory;
            var tasks = new HashSet<Task>();
            var stages = Inline(param.ToQueue(), jobs).ToArray();
            foreach (var stage in stages)
            {
                tasks.Add(
                    factory.StartNew(() => ExecuteStage(stage.Key, stage.Value))
                );
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static IEnumerable<KeyValuePair<IQueue<Object>, IPipelineJob>> Inline(IQueue<Object> firstQueue, IEnumerable<IPipelineJob> jobs)
        {
            var etor = jobs.GetEnumerator();

            if (!etor.MoveNext())
            {
                yield break;
            }

            var kvp = new KeyValuePair<IQueue<Object>, IPipelineJob>(firstQueue, etor.Current);

            yield return kvp;

            var last = etor.Current;

            while (etor.MoveNext())
            {
                kvp = new KeyValuePair<IQueue<Object>, IPipelineJob>(last.Output, etor.Current);

                yield return kvp;

                last = etor.Current;
            }
        }


        private void ExecuteStage(IQueue<Object> input, IPipelineJob job)
        {
            var jobType = job.GetType();
            try
            {
                foreach (var item in input.GetElements())
                {
                    //if (token.IsCancellationRequested) break;
                    job.InternalPerform(item);
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                job.Output.CompleteAdding();
            }
        }
    }
}
