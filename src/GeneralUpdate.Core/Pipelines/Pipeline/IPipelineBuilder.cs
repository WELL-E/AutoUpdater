
using GeneralUpdate.Core.Pipelines.Context;
using GeneralUpdate.Core.Pipelines.Middleware;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GeneralUpdate.Core.Pipelines.Pipeline
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Use(Func<UpdateContext, UpdateDelegate> middleware);

        IPipelineBuilder Launch();
    }
}
