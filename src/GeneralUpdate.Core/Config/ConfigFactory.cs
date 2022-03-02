using GeneralUpdate.Core.Config.Cache;
using GeneralUpdate.Core.Config.Handles;
using GeneralUpdate.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneralUpdate.Core.Config
{
    /// <summary>
    /// Update local configuration file.
    /// </summary>
    public sealed class ConfigFactory
    {
        #region Private Members

        private readonly static object _lock = new object();
        private static ConfigFactory _instance = null;
        private string[] FileTypes = new string[4] { ".db", ".xml", ".ini", ".json" };
        private const string FolderName = "backup";
        private ConfigCache<ConfigEntity> _configCache;
        private string _tempBackupPath;
        private string _targetPath;
        private List<string> _files;
        private bool _disposed = false;

        #endregion

        #region Constructors

        private ConfigFactory()
        {
            _configCache = new ConfigCache<ConfigEntity>();
            _targetPath = _targetPath ?? Environment.CurrentDirectory;
            _tempBackupPath = _tempBackupPath ?? Path.Combine(_targetPath, $"{ FolderName }_{ DateTime.Now.ToShortTimeString() }");
        }

        ~ConfigFactory()
        {
            Dispose();
        }

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods

        public void Deploy()
        {
            if (_configCache.Cache == null) return;

            foreach (var cacheItem in _configCache.Cache)
            {

            }
        }

        public void Cache(IEnumerable<string> files)
        {
            try
            {
                _files = new List<string>(Scan(_targetPath, FileTypes));
                if (_files != null) Cache(_files);
                foreach (var file in files)
                {
                    File.Copy(file, _tempBackupPath);
                    var fileMD5 = FileUtil.GetFileMD5(file);
                    var entity = Handle(file, fileMD5);
                    _configCache.TryAdd(fileMD5, entity);
                }
                _configCache.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"'Cache' error :{ ex.Message } .", ex.InnerException);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_configCache != null)
                {
                    _configCache.Dispose();
                    _configCache = null;
                }

                if (Directory.Exists(_tempBackupPath)) Directory.Delete(_tempBackupPath, true);

                if (_files != null)
                {
                    _files.Clear();
                    _files = null;
                }

                _disposed = true;
            }
            catch (Exception ex)
            {
                _disposed = false;
                throw new Exception($"'Dispose' error :{ ex.Message } .", ex.InnerException);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> Scan(string path, string[] configFormat)
        {
            return null;
        }

        private ConfigEntity Handle(string file, string fileMD5)
        {
            IHandle<object> handle = null;
            var handleEnum = HandleEnum.NONE;
            var entity = new ConfigEntity();
            entity.Path = file;
            entity.MD5 = fileMD5;
            var fileExtension = Path.GetExtension(file);
            switch (fileExtension)
            {
                case ".db":
                    handle = new DBHandle<object>();
                    handleEnum = HandleEnum.DB;
                    break;
                case ".ini":
                    handle = new IniHandle<object>();
                    handleEnum = HandleEnum.INI;
                    break;
                case ".json":
                    handle = new JsonHandle<object>();
                    handleEnum = HandleEnum.JSON;
                    break;
                case ".xml":
                    handle = new XmlHandle<object>();
                    handleEnum = HandleEnum.XML;
                    break;
            }
            entity.Handle = handleEnum;
            entity.Content = handle.Read(file);
            return entity;
        }

        private bool Verify(IEnumerable<string> files)
        {
            return false;
        }

        #endregion
    }
}