using GeneralUpdate.ClientCore.ZipFactory.Events;
using System.Text;

namespace GeneralUpdate.ClientCore.ZipFactory.Factory
{
    public interface IOperation
    {
        void Configs(string sourcePath, string destinationPath, Encoding encoding, bool includeBaseDirectory = false);

        bool CreatZip();

        bool UnZip();

        void OnCompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);

        void OnUnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
    }
}
