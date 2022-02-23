using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using GeneralUpdate.Zip.G7z;
using GeneralUpdate.Zip.GZip;
using System.Text;

namespace GeneralUpdate.Zip
{
    /// <summary>
    /// The compression factory chooses the compressed package format you want to operate .
    /// </summary>
    public class GeneralZipFactory : IFactory
    {
        private BaseCompress _operation;

        public delegate void CompleteEventHandler(object sender, CompleteEventArgs e);

        public event CompleteEventHandler Completed;

        public delegate void UnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);

        public event UnZipProgressEventHandler UnZipProgress;

        public delegate void CompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);

        public event CompressProgressEventHandler CompressProgress;

        /// <summary>
        /// Select archive format .
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFactory CreatefOperate(OperationType type, string sourcePath, string destinationPath, bool includeBaseDirectory = false, Encoding encoding = null)
        {
            switch (type)
            {
                case OperationType.GZip:
                    _operation = new GeneralZip();
                    _operation.Configs(sourcePath, destinationPath, encoding, includeBaseDirectory);
                    break;

                case OperationType.G7z:
                    _operation = new General7z();
                    _operation.Configs(sourcePath, destinationPath, encoding, includeBaseDirectory);
                    break;
            }
            _operation.CompressProgress += OnCompressProgress;
            _operation.UnZipProgress += OnUnZipProgress;
            _operation.Completed += OnCompleted;
            return this;
        }

        private void OnCompleted(object sender, CompleteEventArgs e)
        {
            if (Completed != null) Completed(sender, e);
        }

        private void OnUnZipProgress(object sender, BaseUnZipProgressEventArgs e)
        {
            if (UnZipProgress != null) UnZipProgress(sender, e);
        }

        private void OnCompressProgress(object sender, BaseCompressProgressEventArgs e)
        {
            if (CompressProgress != null) CompressProgress(sender, e);
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