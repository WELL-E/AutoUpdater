using GeneralUpdate.Core.Pipelines.Middleware;

namespace GeneralUpdate.Core.Pipelines.Pipeline
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Use(IMiddleware middleware);

        IPipelineBuilder Launch();
    }
}
