using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GeneralUpdate.Common.Utils
{
    public class SerializeUtil
    {
        public static string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            try
            {
                var formatter = new BinaryFormatter();
                var stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to serialize {0}, reason：{1}", obj, ex.Message));
            }
        }

        public static T Deserialize<T>(string str)
        {
            var obj = default(T);
            if (string.IsNullOrEmpty(str))
                return obj;

            try
            {
                var formatter = new BinaryFormatter();
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to serialize {0}, reason：{1}", obj, ex.Message));
            }
            return obj;
        }
    }
}
