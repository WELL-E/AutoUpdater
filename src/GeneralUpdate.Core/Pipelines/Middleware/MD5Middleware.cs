using GeneralUpdate.Core.Pipelines.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public class MD5Middleware : IMiddleware
    {
        public async Task InvokeAsync(UpdateContext context, UpdateDelegate next) 
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            await next.Invoke(context);
        }
    }
}
