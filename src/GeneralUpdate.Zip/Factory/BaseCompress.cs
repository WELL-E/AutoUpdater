using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    public class BaseCompress : IDisposable
    {
        protected string SOLUTION_BASE_PATH { get; set; }
        protected string SOURSE_PATH { get; set; }

        public BaseCompress() 
        {
            var index = AppDomain.CurrentDomain.BaseDirectory.IndexOf("SharpCompress.Test", StringComparison.OrdinalIgnoreCase);
            SOLUTION_BASE_PATH = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.Substring(0, index));
        }

        public void Verifypath(string soursePath , string destinationPath) 
        {
            if (string.IsNullOrWhiteSpace(soursePath) || string.IsNullOrWhiteSpace(destinationPath)) throw new ArgumentNullException("'Sourse path' or 'Destination path' Is null or empty.");

            if (!File.Exists(soursePath)) throw new Exception("The source file path failed to be accessed or does not exist !");

            if(!Directory.Exists(destinationPath)) throw new Exception("The destination directory does not exist !");
        }

        public void Dispose()
        {
            if (string.IsNullOrWhiteSpace(SOURSE_PATH)) throw new ArgumentNullException(nameof(SOURSE_PATH));
            Directory.Delete(SOURSE_PATH, true);
        }
    }
}