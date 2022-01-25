using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using GeneralUpdate.Zip.G7z;
using GeneralUpdate.Zip.GRAR;
using GeneralUpdate.Zip.GZip;
using System;

namespace GeneralUpdate.Zip
{
    public class GeneralZipFactory : IFactory
    {
        private IOperation _operation;

        public delegate void UnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
        public event UnZipProgressEventHandler UnZipProgress;

        public delegate void CompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);
        public event CompressProgressEventHandler CompressProgress;

        public IFactory Configs(string sourcePath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath)) throw new Exception("'Configs' path not set !");
            _operation.Configs(sourcePath, targetPath);
            return this;
        }

        public IFactory CreatefOperate(OperationType type)
        {
            switch (type)
            {
                case OperationType.GZip:
                    _operation = new GeneralZip();
                    break;
                case OperationType.GRAR:
                    _operation = new GeneralRAR();
                    break;
                case OperationType.G7z:
                    _operation = new General7z();
                    break;
            }
            CompressProgress += OnCompressProgress;
            UnZipProgress += OnUnZipProgress;
            return this;
        }

        private void OnUnZipProgress(object sender, BaseUnZipProgressEventArgs e)
        {
            _operation.OnUnZipProgressEventHandler(sender, e);
        }

        private void OnCompressProgress(object sender, BaseCompressProgressEventArgs e)
        {
            _operation.OnCompressProgressEventHandler(sender, e);
        }

        public IFactory CreatZip()
        {
            _operation.CreatZip();
            return this;
        }

        public IFactory UnZip()
        {
            _operation.UnZip();
            return this;
        }
    }
}
