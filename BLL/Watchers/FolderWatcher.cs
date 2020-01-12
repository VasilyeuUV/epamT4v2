using BLL.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace BLL.Watchers
{


    /// <summary>
    /// Folder Watcher class by FileSystemWatcher
    /// On error create FileSystemWatcher object return null
    /// On watch error return next string:  "!" + error type name
    /// </summary>
    internal sealed class FolderWatcher : ILaunchable, IDisposable
    {
        private static readonly string FILE_FILTER = ConfigurationManager.AppSettings["FileFilter"];

        private FileSystemWatcher _watcher;

        /// <summary>
        /// work will continue as long as the this variable is true
        /// </summary>
        public bool IsLaunched { get; private set; } = false;


        public event EventHandler<string> NewFileDetectedEvent;




        /// <summary>
        /// CTOR
        /// </summary>
        private FolderWatcher(string path)
        {
            this._watcher = new FileSystemWatcher(path, FILE_FILTER);
            this._watcher.IncludeSubdirectories = false;
            this._watcher.NotifyFilter = NotifyFilters.LastWrite;
            this._watcher.Changed += _watcher_Changed;
            this._watcher.Error += _watcher_Error;
        }


        /// <summary>
        /// Create FolderWatcher
        /// </summary>
        /// <param name="path">Watched directory path</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FolderWatcher CreateInstance(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return null; }

            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                if (!folder.Exists) { folder.Create(); }
            }
            catch (Exception)
            {
                return null;
            }     

            return new FolderWatcher(path);
        }

               

        /// <summary>
        /// Launch watching 
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Launch()
        {
            if (this._watcher == null) { return; }
            try
            {
                this._watcher.EnableRaisingEvents = true;
                while (IsLaunched) { Thread.Sleep(1000); }
            }
            catch (Exception)
            {
                this.Stop();
            }
        }

        /// <summary>
        /// Stop FileWatcher
        /// </summary>
        public void Stop()
        {
            if (this._watcher == null) { return; }
            this._watcher.EnableRaisingEvents = false;
            IsLaunched = false;            
        }



        /// <summary>
        /// Chechk for file ready to use.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return inputStream.Length > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region FILESYSTEMWATCHER_EVENTS
        //####################################################################################################################

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            if (IsFileReady(filePath))
            {
                NewFileDetectedEvent?.Invoke(this, filePath);
            }
        }


        private void _watcher_Error(object sender, ErrorEventArgs e)
        {
            string err = "!" + e.GetException().GetType().ToString();
            NewFileDetectedEvent?.Invoke(this, err);       
        }

        #endregion // FILESYSTEMWATCHER_EVENTS




        #region DISPOSING
        //####################################################################################################################

        private bool disposed = false;


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Stop();
            Dispose(true);
            // говорим сборщику мусора, что наш объект уже освободил ресурсы 
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (this._watcher != null)
                    {
                        this._watcher.Changed -= _watcher_Changed;
                        this._watcher.Error -= _watcher_Error;
                        this._watcher.Dispose();
                        this._watcher = null;
                    }
                }

                // освобождаем НЕУПРАВЛЯЕМЫЕ ресурсы 
                // ... 

                disposed = true;
            }
        }


        //// деструктор класса 
        //~FolderWatcher()
        //{
        //    Dispose(false);
        //}



        #endregion // DISPOSING


    }
}
