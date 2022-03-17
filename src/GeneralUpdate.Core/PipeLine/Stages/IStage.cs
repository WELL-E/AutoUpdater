using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.PipeLine.Stages
{
    interface IStage
    {
        IPipelineJob Job { get; }
        IStage Next { get; set; }
        IStage First { get; set; }
    }
}
