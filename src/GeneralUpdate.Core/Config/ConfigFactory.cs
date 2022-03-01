using GeneralUpdate.Core.Config.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralUpdate.Core.Config
{
    /// <summary>
    /// Update local configuration file.
    /// </summary>
    public sealed class ConfigFactory : AbstractFactory
    {
        private readonly static object _lock = new object();
        private static ConfigFactory _instance = null;
        private bool _disposed;
        private string _tempBackupPath;

        private ConfigFactory() { }

        ~ConfigFactory() { }

        public static ConfigFactory Instance 
        {
            get 
            { 
                if (_instance == null) 
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        public override void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            base.Dispose();
        }
    }
}
