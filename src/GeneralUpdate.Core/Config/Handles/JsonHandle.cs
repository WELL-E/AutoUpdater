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
    /// <summary>
    /// JSON configuration file processing class .
    /// </summary>
    /// <typeparam name="TContent">json configuration file content.</typeparam>
    public class JsonHandle<TContent> : IHandle<TContent>, IAwaiter<JsonHandle<TContent>> where TContent : class
    {
        private bool _isCompleted;
        private Exception _exception;

        public bool IsCompleted { get => _isCompleted; private set => _isCompleted = value; }

        public void OnCompleted(Action continuation)
        {
            if (continuation != null) continuation.Invoke();
        }

        /// <summary>
        /// Read the content of the configuration file according to the path .
        /// </summary>
        /// <param name="path">file path.</param>
        /// <returns>file content.</returns>
        public Task<TContent> Read(string path)
        {
            var jsonText = File.ReadAllText(path);
            return Task.FromResult(JsonConvert.DeserializeObject<TContent>(jsonText));
        }

        /// <summary>
        /// Write the processed content to the configuration file .
        /// </summary>
        /// <param name="path">file path.</param>
        /// <param name="content">file content.</param>
        /// <returns>is done.</returns>
        public Task<bool> Write(string path, TContent content)
        {
            try
            {
                var targetObj = Read(path);
                CopyValue(content, targetObj);
                File.WriteAllText(path, JsonConvert.SerializeObject(targetObj));
                return Task.FromResult(IsCompleted = true);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Iterate over objects and copy values .
        /// </summary>
        /// <typeparam name="T">json object .</typeparam>
        /// <param name="source">original configuration file .</param>
        /// <param name="target">latest configuration file .</param>
        private void CopyValue<T>(TContent source, T target) where T : class
        {
            try
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
            catch (Exception ex)
            {
                _exception = ex;
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