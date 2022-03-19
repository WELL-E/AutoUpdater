using GeneralUpdate.Core.Pipelines.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Pipelines.MiddlewareResolver
{
    /// <summary>
    /// A default implementation of <see cref="IMiddlewareResolver"/> that creates
    /// instances using the <see cref="System.Activator"/>.
    /// </summary>
    public class ActivatorMiddlewareResolver
    {
        public static object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
