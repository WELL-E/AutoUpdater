using GeneralUpdate.Common.CustomAwaiter;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Config.Handles
{
    public class JsonHandle<TContent> : IHandle<TContent>, IAwaiter<JsonHandle<TContent>> where TContent : class
    {
        private bool _isCompleted;
        private Exception _exception;

        public bool IsCompleted { get => _isCompleted; private set => _isCompleted = value; }

        public void OnCompleted(Action continuation)
        {
            if(continuation != null) continuation.Invoke();
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
                return IsCompleted = true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private void CopyValueToTarget<T>(T source, T target) where T : class
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

        public JsonHandle<TContent> GetAwaiter()
        {
            return this;
        }

        public JsonHandle<TContent> GetResult()
        {
            if (_exception != null) ExceptionDispatchInfo.Capture(_exception).Throw();
            return this;
        }

        public async Task AsTask(JsonHandle<TContent> awaiter)
        {
            await awaiter;
        }
    }
}