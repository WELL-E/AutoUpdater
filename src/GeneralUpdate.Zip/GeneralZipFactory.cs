using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using GeneralUpdate.Zip.G7z;
using GeneralUpdate.Zip.GZip;
using System;

namespace GeneralUpdate.Zip
{
    /// <summary>
    /// The compression factory chooses the compressed package format you want to operate .
    /// </summary>
    public class GeneralZipFactory : IFactory
    {
        private IOperation _operation;

        public delegate void UnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
        public event UnZipProgressEventHandler UnZipProgress;

        public delegate void CompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);
        public event CompressProgressEventHandler CompressProgress;

        /// <summary>
        /// configuration path .
        /// </summary>
        /// <param name="sourcePath">source path </param>
        /// <param name="targetPath"> target path</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IFactory Configs(string sourcePath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath)) throw new Exception("'Configs' path not set !");
            _operation.Configs(sourcePath, targetPath);
            return this;
        }

        /// <summary>
        /// Select archive format .
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFactory CreatefOperate(OperationType type)
        {
            switch (type)
            {
                case OperationType.GZip:
                    _operation = new GeneralZip();
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

        /// <summary>
        /// Creat zip.
        /// </summary>
        /// <returns></returns>
        public IFactory CreatZip()
        {
            _operation.CreatZip();
            return this;
        }

        /// <summary>
        /// Un zip.
        /// </summary>
        /// <returns></returns>
        public IFactory UnZip()
        {
            _operation.UnZip();
            return this;
        }
    }
}
