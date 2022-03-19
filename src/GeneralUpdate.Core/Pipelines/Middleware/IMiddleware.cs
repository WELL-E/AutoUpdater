using GeneralUpdate.Core.Pipelines.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(UpdateContext context, UpdateDelegate next);
    }
}
