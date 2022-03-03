using GeneralUpdate.Common.CustomAwaiter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Config.Handles
{
    public class JsonHandle<TContent> : IHandle<TContent> , IAwaiter<TContent> where TContent : class
    {
        public bool IsCompleted => throw new NotImplementedException();

        public TContent GetResult()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public TContent Read(string path)
        {
            var jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<TContent>(jsonText);
        }

        public bool Write(string path, TContent content)
        {
            try
            {
                var targetObj = Read(path);
                CopyValueToTarget(content, targetObj);
                File.WriteAllText(path, JsonConvert.SerializeObject(targetObj));
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static void CopyValueToTarget<T>(T source, T target) where T : class
        {
            Type type = source.GetType();
            var fields = type.GetRuntimeFields().ToList();
            foreach (var field in fields)
            {
                field.SetValue(target, field.GetValue(source));
            }

            var properties = type.GetRuntimeProperties().ToList();
            foreach (var property in properties)
            {
                property.SetValue(target, property.GetValue(source));
            }
        }
    }
}
