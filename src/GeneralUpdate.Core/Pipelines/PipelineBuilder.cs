using GeneralUpdate.Core.Pipelines.Context;
using GeneralUpdate.Core.Pipelines.Middleware;
using GeneralUpdate.Core.Pipelines.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines
{
    public class PipelineBuilder<TContext> : IPipelineBuilder where TContext : class
    {
        private IList<Func<UpdateContext, UpdateDelegate>> _components = new List<Func<UpdateContext, UpdateDelegate>>();
        private TContext _context;


        public PipelineBuilder(TContext context)
        {
            _context = context;
        }

        public IPipelineBuilder Use(Func<UpdateContext, UpdateDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        public IPipelineBuilder Launch()
        {
            UpdateDelegate app = context =>
            {
                // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
                // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
                //var endpoint = context.GetEndpoint();
                //var endpointRequestDelegate = endpoint?.RequestDelegate;
                //if (endpointRequestDelegate != null)
                //{
                //    var message =
                //        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint!.DisplayName}'. " +
                //        $"Please register the EndpointMiddleware using '{nameof(IPipelineBuilder)}.UseEndpoints(...)' if using " +
                //        $"routing.";
                //    throw new InvalidOperationException(message);
                //}
                return Task.CompletedTask;
            };

            //for (var c = _components.Count - 1; c >= 0; c--)
            //    app = _components[c](app);

            _components[0].Invoke(_context as UpdateContext);
            return this;
        }

    }
}
