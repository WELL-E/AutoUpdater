using GeneralUpdate.Zip.Factory;
using GeneralUpdate.Zip.G7z;
using GeneralUpdate.Zip.GRAR;
using GeneralUpdate.Zip.GZip;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Zip
{
    internal class GeneralZipFactory : IFactory
    {
        private IOperation _operation;
        private string _sourcePath, _targetPath;

        public IFactory Configs(string sourcePath, string targetPath)
        {
            _sourcePath = sourcePath;
            _targetPath = targetPath;
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
            return this;
        }

        public IFactory CreatZip()
        {
            _operation.CreatZip();
            return this;
        }

        public IFactory Zip()
        {
            _operation.Zip();
            return this;
        }
    }
}
