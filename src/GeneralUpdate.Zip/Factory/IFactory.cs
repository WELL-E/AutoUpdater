using GeneralUpdate.Common.Models;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    public interface IFactory
    {
        IFactory CreatefOperate(OperationType type, string sourcePath, string destinationPath, bool includeBaseDirectory = false, Encoding encoding = null);

        IFactory CreatZip();

        IFactory UnZip();
    }
}