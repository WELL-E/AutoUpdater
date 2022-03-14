using GeneralUpdate.Common.Utils;
using GeneralUpdate.Differential.Common;
using GeneralUpdate.Differential.Config.Cache;
using GeneralUpdate.Differential.Config.Handles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential.Config
{
    /// <summary>
    /// Update local configuration file.[Currently only files with a depth of 1 are supported.]
    /// Currently only json files are supported.
    /// </summary>
    public sealed class ConfigFactory : IDisposable
    {
        #region Private Members

        private const string FolderName = "backup";
        private ConfigCache<ConfigEntity> _configCache;
        private string _tempBackupPath;
        private string _scanPath;
        private List<string> _files;
        private bool _disposed = false;

        #endregion Private Members

        #region Constructors

        public ConfigFactory()
        {
            _configCache = new ConfigCache<ConfigEntity>();
            _scanPath = _scanPath ?? Environment.CurrentDirectory;
            _tempBackupPath = _tempBackupPath ?? Path.Combine(_scanPath, $"{ FolderName }");
        }

        ~ConfigFactory()
        {
            Dispose();
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Deploy configuration file.
        /// </summary>
        public async Task Deploy()
        {
            try
            {
                if (_configCache.Cache != null)
                {
                    foreach (var cacheItem in _configCache.Cache)
                    {
                        var value = cacheItem.Value;
                        if (value == null) continue;
                        var handle = InitHandle<ConfigEntity>(value.Handle);
                        await handle.Write(value.Path, value);
                    }
                    Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Deploy config error : { ex.Message } .", ex.InnerException);
            }
        }

        /// <summary>
        /// Scan configuration files and cache, backup.
        /// </summary>
        public async Task Scan(string scanPath = null)
        {
            try
            {
                _scanPath = scanPath ?? Environment.CurrentDirectory;
                List<string> files = new List<string>();
                Find(_scanPath, ref files, Filefilter.Temp);
                if (files.Count == 0) return;
                _files = files;
                await Cache(_files);
            }
            catch (Exception ex)
            {
                throw new Exception($"Scan config files error : { ex.Message } .", ex.InnerException);
            }
        }

        /// <summary>
        /// release all resources.
        /// </summary>
        /// <exception cref="Exception">dispose exception</exception>
        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (Directory.Exists(_tempBackupPath)) Directory.Delete(_tempBackupPath, true);

                if (_configCache != null)
                {
                    _configCache.Dispose();
                    _configCache = null;
                }

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

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Find matching files recursively.
        /// </summary>
        /// <param name="directory">root directory</param>
        /// <param name="files">result file list</param>
        private void Find(string rootDirectory, ref List<string> files, IEnumerable<string> filter)
        {
            var rootDirectoryInfo = new DirectoryInfo(rootDirectory);
            foreach (var file in rootDirectoryInfo.GetFiles())
            {
                var extensionName = Path.GetExtension(file.Name);
                if (!Filefilter.Temp.Contains(extensionName)) continue;
                var fullName = file.FullName;
                files.Add(fullName);
            }
            foreach (var dir in rootDirectoryInfo.GetDirectories())
            {
                Find(dir.FullName, ref files, filter);
            }
        }

        /// <summary>
        /// All resources are cached and backed up.
        /// </summary>
        /// <param name="files"></param>
        /// <exception cref="Exception"></exception>
        private async Task Cache(IEnumerable<string> files)
        {
            if (_files == null) return;
            if (!Directory.Exists(_tempBackupPath)) Directory.CreateDirectory(_tempBackupPath);
            try
            {
                foreach (var file in files)
                {
                    var tempPath = Path.Combine(_tempBackupPath, Path.GetFileName(file));
                    File.Copy(file, tempPath, true);
                    var fileMD5 = FileUtil.GetFileMD5(file);
                    var entity = await Handle(file, fileMD5);
                    _configCache.TryAdd(fileMD5, entity);
                }
                _configCache.Build();
            }
            catch (Exception ex)
            {
                throw new Exception($"'Cache' error :{ ex.Message } .", ex.InnerException);
            }
        }

        /// <summary>
        /// Process file content.
        /// </summary>
        /// <param name="file">file path</param>
        /// <param name="fileMD5">md5</param>
        /// <returns></returns>
        private async Task<ConfigEntity> Handle(string file, string fileMD5)
        {
            var entity = new ConfigEntity();
            entity.Path = file;
            entity.MD5 = fileMD5;
            entity.Handle = ToEnum(file);
            entity.Content = await InitHandle<object>(entity.Handle).Read(file);
            return entity;
        }

        /// <summary>
        /// Initialize the corresponding file processing object.
        /// </summary>
        /// <typeparam name="TEntity">file entity</typeparam>
        /// <param name="handleEnum">handle enum</param>
        /// <returns>handle</returns>
        private IHandle<TEntity> InitHandle<TEntity>(HandleEnum handleEnum) where TEntity : class
        {
            IHandle<TEntity> handle = null;
            switch (handleEnum)
            {
                case HandleEnum.DB:
                    handle = new DBHandle<TEntity>();
                    break;

                case HandleEnum.INI:
                    handle = new IniHandle<TEntity>();
                    break;

                case HandleEnum.JSON:
                    handle = new JsonHandle<TEntity>();
                    break;

                case HandleEnum.XML:
                    handle = new XmlHandle<TEntity>();
                    break;
            }
            return handle;
        }

        /// <summary>
        /// Convert enumeration value according to file type.
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns>handle enum</returns>
        private HandleEnum ToEnum(string file)
        {
            var fileExtension = Path.GetExtension(file);
            var handleEnum = HandleEnum.NONE;
            switch (fileExtension)
            {
                case ".db":
                    handleEnum = HandleEnum.DB;
                    break;

                case ".ini":
                    handleEnum = HandleEnum.INI;
                    break;

                case ".json":
                    handleEnum = HandleEnum.JSON;
                    break;

                case ".xml":
                    handleEnum = HandleEnum.XML;
                    break;
            }
            return handleEnum;
        }

        #endregion Private Methods
    }
}