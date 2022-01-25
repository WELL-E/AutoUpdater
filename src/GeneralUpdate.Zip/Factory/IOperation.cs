using GeneralUpdate.Zip.Events;
namespace GeneralUpdate.Zip.Factory
{
    internal interface IOperation
    {
        void Configs(string sourcePath,string targetPath);

        bool CreatZip();

        bool UnZip();

        void OnCompressProgressEventHandler(object sender, BaseCompressProgressEventArgs e);

        void OnUnZipProgressEventHandler(object sender, BaseUnZipProgressEventArgs e);
    }
}
