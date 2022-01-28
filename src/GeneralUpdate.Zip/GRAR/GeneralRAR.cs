using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip.GRAR
{
    internal class GeneralRAR : IOperation
    {
        public void Configs(string sourcePath, string targetPath)
        {
            throw new NotImplementedException();
        }

        public bool CreatZip()
        {
            throw new NotImplementedException();
        }

        public void OnCompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnUnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool UnZip()
        {
            throw new NotImplementedException();
        }
    }
}
