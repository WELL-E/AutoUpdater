using GeneralUpdate.Zip.Events;
using System.Text;

namespace GeneralUpdate.Zip.Factory
{
    internal interface IOperation
    {
        void Configs(string sourcePath,string destinationPath);

        void Configs(string sourcePath, string destinationPath, Encoding encoding);

        bool CreatZip();

        bool UnZip();

        void OnCompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);

        void OnUnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
    }
}
