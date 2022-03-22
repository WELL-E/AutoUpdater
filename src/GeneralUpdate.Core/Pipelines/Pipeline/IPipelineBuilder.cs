using GeneralUpdate.Core.Pipelines.Middleware;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Pipeline
{
    public interface IPipelineBuilder
    {
        IPipelineBuilder Use(IMiddleware middleware);

        Task<IPipelineBuilder> Launch();
    }
}