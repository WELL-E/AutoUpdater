using GeneralUpdate.Core.PipeLine;
using GeneralUpdate.Core.PipeLine.Jobs;
using GeneralUpdate.Core.PipeLine.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate.WpfNet6_Sample
{
    public class TestClass : AsyncJob<int, double>
    {
        public override double Perform(int param)
        {
            return param / 2;
        }
    }

    public class TestClass2 : AsyncJob<double, double>
    {
        public override double Perform(double param)
        {
            return param / 2;
        }
    }
}
