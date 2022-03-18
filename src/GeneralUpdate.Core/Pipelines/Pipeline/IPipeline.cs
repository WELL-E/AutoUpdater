using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Pipelines.Pipeline
{
    public interface IPipeline
    {
        Task Create();
        Task Use();
        Task Run();
    }
}
