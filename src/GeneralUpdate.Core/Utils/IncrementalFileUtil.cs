using GeneralUpdate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUpdate.Core.Utils
{
    public class IncrementalFileUtil
    {
        private static readonly object _lockObj = new object();
        private static IncrementalFileUtil _instance;
        private List<FileBase> _oldFiles;
        private List<FileBase> _newFiles;
        private List<FileBase> _incrementalFiles;

        public static IncrementalFileUtil Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new IncrementalFileUtil();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Backups(string targetPath) 
        {
            try
            {
                if (_incrementalFiles == null) return;

                foreach (var file in _incrementalFiles)
                {
                    FileUtil.CopyFile(file.Path, $"{ targetPath }\\{ file.Name }");
                    file.Path = $"{ targetPath }\\{ file.Name }";
                }
            }
            catch (Exception)
            {

            }
        }

        public void ClearBackups() 
        {
            if (_incrementalFiles == null) return;

            foreach (var file in _incrementalFiles)
            {
                FileUtil.DeleteFile(file.TempPath);
            }
        }

        public void RollBack(string targetPath,string backupsPath) 
        {
            try
            {
                if (_incrementalFiles == null) return;

                foreach (var file in _incrementalFiles)
                {
                    if (file.Status == FileStatus.Old)
                    {
                        string revertFile = $"{ targetPath }\\{ file.Name }";
                        if (FileUtil.DeleteFile(revertFile))
                        {
                            FileUtil.CopyFile(file.Path, revertFile);
                        }
                    }
                    else if (file.Status == FileStatus.New)
                    {
                        string revertFile = $"{ targetPath }\\{ file.Name }";
                        FileUtil.DeleteFile(revertFile);
                    }
                }
                FileUtil.DelectDir(backupsPath);
            }
            catch (Exception)
            {

            }
        }

        public List<FileBase> GetIncrementalFiles() 
        {
            if (_oldFiles == null)
            {
                throw new Exception("Component internal exception, old files not set!");
            }

            if (_newFiles == null)
            {
                throw new Exception("Component internal exception, new files not set!");
            }

            _incrementalFiles = new List<FileBase>();

            foreach (var newfile in _newFiles)
            {

                var file = _oldFiles.FirstOrDefault(i=>i.Name.Equals(newfile.Name));
                if (file != null)
                {
                    newfile.Status = FileStatus.Old;
                    _incrementalFiles.Add(file);
                }
                else
                {
                    newfile.Status = FileStatus.New;
                    _incrementalFiles.Add(newfile);
                }
            }
            return _incrementalFiles;
        }

        public void GetOldFileinfo(string path) 
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("Component internal exception, old folder path not set not set!");
            }
            _oldFiles = FileUtil.GetFiles(path);
        }

        public void GetNewFileinfo(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("Component internal exception, new folder path not set not set!");
            }
            _newFiles = FileUtil.GetFiles(path);
        }
    }
}
