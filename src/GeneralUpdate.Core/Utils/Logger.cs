using log4net;
using System;

namespace AutoUpdate.Core.Utils
{

    public class Logger
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(string info) {
            Log.Info(info);
        }

        public static void Error(string info , Exception ex = null) {
            Log.Error(info, ex);
        }
    }
}
