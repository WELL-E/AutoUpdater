using GeneralUpdate.Core.Pipelines.Context;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(BaseContext context, MiddlewareStack stack);
    }
}
