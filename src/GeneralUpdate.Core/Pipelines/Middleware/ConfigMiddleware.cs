using GeneralUpdate.Core.Pipelines.Context;
using GeneralUpdate.Core.Update;
using GeneralUpdate.Differential.Config;
using System;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public class ConfigMiddleware : IMiddleware
    {
        public async Task InvokeAsync(BaseContext context, MiddlewareStack stack)
        {
            try
            {
                context.OnProgressEventAction(this, ProgressType.Config, "Update configuration file ...");
                await ConfigFactory.Instance.Deploy();
                var node = stack.Pop();
                if (node != null) await node.Next.Invoke(context, stack);
            }
            catch (Exception ex)
            {
                var exception = new Exception($"An exception occurred while updating the configuration file : { ex.Message } !", ex.InnerException);
                context.OnExceptionEventAction(this, exception);
                throw exception;
            }
        }
    }
}