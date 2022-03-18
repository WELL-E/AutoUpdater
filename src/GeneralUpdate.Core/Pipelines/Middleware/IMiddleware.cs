using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Middleware
{
    public interface IMiddleware
    {
        Task Invoke();
    }
}
