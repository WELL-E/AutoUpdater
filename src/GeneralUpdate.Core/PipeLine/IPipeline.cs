using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine
{
    public interface IPipeline<TParam>
    {
        /// <summary>
        /// Runs the pipeline with the specified params.
        /// </summary>
        /// <param name="param">The param.</param>
        void Run(IEnumerable<TParam> param);
    }
}
