using System;
using System.IO;

namespace AutoUpdater.Utils
{
    public class Loger
    {
        private static readonly string FilePath;

        static Loger()
        {
            FilePath = AppDomain.CurrentDomain.BaseDirectory + "update.log";
        }

        public static void Print(string msg)
        {
            using (var sw = File.AppendText(FilePath))
            {
                var logLine = string.Format("{0:G}: {1}.", DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
        }
    }
}
