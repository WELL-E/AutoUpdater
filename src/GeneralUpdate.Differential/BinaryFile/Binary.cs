using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneralUpdate.Differential.BinaryFile
{
    /// <summary>
    /// https://github.com/lindexi/lindexi_gd/tree/739bb867bd62d9356dc5a3d189e9e1d63daf4a69/LwufxgbaDljqkx
    /// https://blog.lindexi.com/post/%E4%BA%8C%E8%BF%9B%E5%88%B6%E6%95%B0%E6%8D%AE%E5%B7%AE%E5%BC%82%E7%AE%97%E6%B3%95-%E7%94%A8%E4%BA%8E%E5%87%8F%E5%B0%8FOTA%E5%86%85%E5%AE%B9.html
    /// </summary>
    public class Binary
    {
        public static List<IBinaryFile> GetBinary(byte[] oldFile, byte[] newFile)
        {
            var file = new List<IBinaryFile>();
            IBinaryFile binaryFile = null;

            for (int i = 0; i < newFile.Length; i++)
            {
                Console.WriteLine($"{i}/{newFile.Length}");

                (int findIndex, int findLength) = FindBinary(oldFile, newFile, i);
                if (findIndex < 0)
                {
                    //没有在之前文件找到数据，这部分数据是只有新文件
                    if (binaryFile is null)
                    {
                        binaryFile = new NewFile();
                    }

                    ((NewFile)binaryFile).Data.Add(newFile[i]);
                }
                else
                {
                    if (binaryFile is NewFile)
                    {
                        file.Add(binaryFile);
                        binaryFile = null;
                    }

                    file.Add(new OldFile()
                    {
                        OldFileIndex = findIndex,
                        Length = findLength
                    });

                    i += findLength - 1;
                }
            }

            if (binaryFile != null)
            {
                file.Add(binaryFile);
            }

            return file;
        }

        public static List<byte> GetNewFile(byte[] oldFile, List<IBinaryFile> file)
        {
            var newFile = new List<byte>();

            foreach (var temp in file)
            {
                if (temp is NewFile NewFile)
                {
                    newFile.AddRange(NewFile.Data);
                }

                if (temp is OldFile OldFile)
                {
                    var span = new Span<byte>(oldFile, OldFile.OldFileIndex, OldFile.Length);

                    newFile.AddRange(span.ToArray());
                }
            }
            return newFile;
        }

        public static (int findIndex, int findLength) FindBinary(byte[] oldFile, byte[] newFile, int newFileIndex)
        {
            var findLength = 8;

            var startIndex = 0;

            var findIndex = FindBinary(oldFile, newFile, newFileIndex, findLength, startIndex);

            if (findIndex < 0)
            {
                return (-1, 0);
            }
            else
            {
                while (true)
                {
                    var currentFindIndex = findIndex;

                    while (true)
                    {
                        findLength++;
                        if (oldFile.Length > currentFindIndex + findLength - 1)
                        {
                            if (newFile.Length > newFileIndex + findLength - 1)
                            {
                                if (oldFile[currentFindIndex + findLength - 1] == newFile[newFileIndex + findLength - 1])
                                {
                                    continue;
                                }
                            }
                        }

                        break;
                    }

                    startIndex = findIndex;
                    findIndex = FindBinary(oldFile, newFile, newFileIndex, findLength, startIndex);

                    if (findIndex < 0)
                    {
                        return (currentFindIndex, findLength - 1);
                    }

                }
            }
        }

        public static int FindBinary(byte[] oldFile, byte[] newFile, int newFileIndex, int findLength, int startIndex)
        {
            if (newFile.Length <= newFileIndex + findLength)
            {
                return -1;
            }

            var arrayFind = new Span<byte>(newFile, newFileIndex, findLength);
            var findIndex = TryFindNewFile(oldFile, arrayFind, startIndex);
            return findIndex;
        }

        public static int TryFindNewFile(byte[] newFile, Span<byte> arrayFind, int startIndex)
        {
            var findLength = arrayFind.Length;
            for (int i = startIndex; i < newFile.Length; i++)
            {
                if (newFile.Length < i + findLength)
                {
                    return -1;
                }

                var source = new Span<byte>(newFile, i, findLength);

                if (Equals(source, arrayFind))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool Equals(Span<byte> source, Span<byte> arrayFind)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != arrayFind[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static List<IBinaryFile> Deserialize(byte[] binary)
        {
            List<IBinaryFile> file = new List<IBinaryFile>();

            var binaryReader = new BinaryReader(new MemoryStream(binary));

            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                var n = binaryReader.ReadInt32();
                if (n < 0)
                {
                    n = -n;
                    var data = new List<byte>(n);
                    for (int i = 0; i < n; i++)
                    {
                        data.Add(binaryReader.ReadByte());
                    }

                    file.Add(new NewFile()
                    {
                        Data = data
                    });
                }
                else
                {
                    var oldFileIndex = n;
                    var length = binaryReader.ReadInt32();
                    file.Add(new OldFile()
                    {
                        OldFileIndex = oldFileIndex,
                        Length = length
                    });
                }
            }
            return file;
        }

        public static byte[] Serialize(List<IBinaryFile> file)
        {
            var stream = new MemoryStream();
            var binaryWriter = new BinaryWriter(stream);

            foreach (var temp in file)
            {
                if (temp is NewFile NewFile)
                {
                    binaryWriter.Write(-NewFile.Data.Count);
                    binaryWriter.Write(NewFile.Data.ToArray());
                }

                if (temp is OldFile OldFile)
                {
                    binaryWriter.Write(OldFile.OldFileIndex);
                    binaryWriter.Write(OldFile.Length);
                }
            }
            return stream.ToArray();
        }
    }
}
