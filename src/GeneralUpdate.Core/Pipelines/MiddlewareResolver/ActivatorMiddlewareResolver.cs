using System;

namespace GeneralUpdate.Core.Pipelines.MiddlewareResolver
{
    public class ActivatorMiddlewareResolver
    {
        public static object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}