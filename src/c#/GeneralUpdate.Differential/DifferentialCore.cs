using GeneralUpdate.Core.Utils;
using GeneralUpdate.Differential.Binary;
using GeneralUpdate.Differential.Common;
using GeneralUpdate.Zip;
using GeneralUpdate.Zip.Events;
using GeneralUpdate.Zip.Factory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Differential
{
    public sealed class DifferentialCore
    {
        #region Private Members

        /// <summary>
        /// Differential file format .
        /// </summary>
        private const string PATCH_FORMAT = ".patch";
        /// <summary>
        /// Folder dependencies file.
        /// </summary>
        private const string DEPEND_FORMAT = ".gdep";

        private const string PATCHS = "patchs";
        private List<FolderDepend> _depends;
        private static readonly object _lockObj = new object();
        private static DifferentialCore _instance;

        private Action<object, BaseCompressProgressEventArgs> _compressProgressCallback;

        #endregion Private Members

        #region Constructors

        private DifferentialCore()
        { }

        #endregion Constructors

        #region Public Properties

        public static DifferentialCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DifferentialCore();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Generate patch file [Cannot contain files with the same name but different extensions] .
        /// </summary>
        /// <param name="appPath">Previous version folder path .</param>
        /// <param name="targetPath">Recent version folder path.</param>
        /// <param name="patchPath">Store discovered incremental update files in a temporary directory .</param>
        /// <param name="compressProgressCallback">Incremental package generation progress callback function.</param>
        /// <param name="type">7z or zip</param>
        /// <param name="encoding">Incremental packet encoding format .</param>
        /// <returns></returns>
        public async Task Clean(string appPath, string targetPath, string patchPath = null, Action<object, BaseCompressProgressEventArgs> compressProgressCallback = null, OperationType type = OperationType.GZip, Encoding encoding = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(patchPath)) patchPath = Path.Combine(Environment.CurrentDirectory, PATCHS);
                if (!Directory.Exists(patchPath)) Directory.CreateDirectory(patchPath);

                //Take the left tree as the center to match the files that are not in the right tree .
                var tupleResult = FileUtil.Compare(targetPath, appPath);
                _depends = new List<FolderDepend>();
                //Binary differencing of like terms .
                foreach (var file in tupleResult.Item2)
                {
                    var depFullName = file.FullName.Replace(targetPath, "");
                    _depends.Add(new FolderDepend(file.Name, depFullName));
                    var oldfile = Path.Combine(appPath, file.Name);
                    var newfile = file.FullName;
                    var extensionName = Path.GetExtension(file.FullName);
                    if (File.Exists(oldfile) && File.Exists(newfile) && !Filefilter.Diff.Contains(extensionName))
                    {
                        //Generate the difference file to the difference directory .
                        await new BinaryHandle().Clean(Path.Combine(appPath, file.Name), file.FullName,
                            Path.Combine(patchPath, $"{ Path.GetFileNameWithoutExtension(file.Name) }{ PATCH_FORMAT }"));
                    }
                    else
                    {
                        File.Copy(newfile, Path.Combine(patchPath, Path.GetFileName(newfile)), true);
                    }
                }
                string depJson = JsonConvert.SerializeObject(_depends);
                File.WriteAllText(Path.Combine(patchPath, $"{ DateTime.Now.ToString("yyyyMMdd") }{DEPEND_FORMAT}"), depJson, encoding ?? Encoding.UTF8);
                _compressProgressCallback = compressProgressCallback;
                var factory = new GeneralZipFactory();
                if (_compressProgressCallback != null) factory.CompressProgress += OnCompressProgress;
                //The update package exists in the 'target path' directory.
                factory.CreatefOperate(type, patchPath, targetPath, true, encoding).CreatZip();
            }
            catch (Exception ex)
            {
                throw new Exception($"Generate error : { ex.Message } !", ex.InnerException);
            }
        }

        /// <summary>
        /// Apply patch [Cannot contain files with the same name but different extensions] .
        /// </summary>
        /// <param name="appPath">Client application directory .</param>
        /// <param name="patchPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Drity(string appPath, string patchPath, Encoding encoding = null)
        {
            if (!Directory.Exists(appPath) || !Directory.Exists(patchPath)) return;
            try
            {
                if (string.IsNullOrWhiteSpace(patchPath) || string.IsNullOrWhiteSpace(appPath))
                    throw new ArgumentNullException(nameof(appPath));

                var patchFiles = FileUtil.GetAllFiles(patchPath);
                var oldFiles = FileUtil.GetAllFiles(appPath);
                //get dep
                var depFile = patchFiles.FirstOrDefault(f=> Path.GetExtension(f.Name).Equals(DEPEND_FORMAT));
                var depJson = FileUtil.GetJsonFile(depFile.FullName, encoding ?? Encoding.UTF8);
                var depObj = JsonConvert.DeserializeObject<List<FolderDepend>>(depJson);
                foreach (var oldFile in oldFiles)
                {
                    //Only the difference file (.patch) can be updated here.
                    var findFile = patchFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name).Equals(Path.GetFileNameWithoutExtension(oldFile.Name)));
                    var depContent = depObj.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name).Equals(Path.GetFileNameWithoutExtension(oldFile.Name)));
                    if (findFile != null && depContent != null)
                    {
                        var extensionName = Path.GetExtension(findFile.FullName);
                        if (!extensionName.Equals(PATCH_FORMAT)) continue;
                        var tempFullName = appPath + depContent.FullName;
                        await DrityPatch(tempFullName, findFile.FullName);
                    }
                }
                //Update does not include files or copies configuration files.
                await DrityUnkonw(appPath, patchPath, depObj);
            }
            catch (Exception ex)
            {
                throw new Exception($"Drity error : { ex.Message } !", ex.InnerException);
            }
        }

        /// <summary>
        /// Apply patch file .
        /// </summary>
        /// <param name="appPath">Client application directory .</param>
        /// <param name="patchPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task DrityPatch(string appPath, string patchPath)
        {
            try
            {
                if (!File.Exists(appPath) || !File.Exists(patchPath)) return;
                var newPath = Path.Combine(Path.GetDirectoryName(appPath), $"{ Path.GetRandomFileName() }_{ Path.GetFileName(appPath) }");
                await new BinaryHandle().Drity(appPath, newPath, patchPath);
            }
            catch (Exception ex)
            {
                throw new Exception($"RevertFile error : { ex.Message } !", ex.InnerException);
            }
        }

        /// <summary>
        /// Add new files .
        /// </summary>
        /// <param name="appPath">Client application directory .</param>
        /// <param name="patchPath"></param>
        private Task DrityUnkonw(string appPath, string patchPath, List<FolderDepend> depObj)
        {
            try
            {
                var listExcept = FileUtil.Compare(patchPath, appPath);
                foreach (var file in listExcept.Item2)
                {
                    var extensionName = Path.GetExtension(file.FullName);
                    if (Filefilter.Diff.Contains(extensionName)) continue;
                    var depContent = depObj.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name).Equals(Path.GetFileNameWithoutExtension(file.Name)));
                    var tempFullName = appPath + depContent.FullName;
                    var tempDir = Path.GetDirectoryName(tempFullName);
                    if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                    File.Copy(file.FullName, tempFullName , true);
                }
                if (Directory.Exists(patchPath)) Directory.Delete(patchPath, true);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception($" DrityNew error : { ex.Message } !", ex.InnerException);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCompressProgress(object sender, BaseCompressProgressEventArgs e)=> _compressProgressCallback(sender, e);

        #endregion Private Methods
    }
}