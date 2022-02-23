namespace GeneralUpdate.Zip.Events
{
    public class BaseUnZipProgressEventArgs
    {
        public long Size { get; set; }

        public int Index { get; set; }

        public int Count { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }
    }
}