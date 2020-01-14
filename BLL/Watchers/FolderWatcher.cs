using BLL.Interfaces;
using FDAL.Versions;
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
        private FileSystemWatcher _watcher;

        /// <summary>
        /// work will continue as long as the this variable is true
        /// </summary>
        public bool IsLaunched { get; private set; } = false;


        public event EventHandler<string> NewFileDetectedEvent;




        /// <summary>
        /// CTOR
        /// </summary>
        private FolderWatcher(string path, string filter)
        {
            this._watcher = new FileSystemWatcher(path, filter);
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
        public static FolderWatcher CreateInstance(string path, string filter)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(filter)) { return null; }

            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                if (!folder.Exists) { folder.Create(); }
            }
            catch (Exception)
            {
                return null;
            }     

            return new FolderWatcher(path, filter);
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



        #region FILESYSTEMWATCHER_EVENTS
        //####################################################################################################################

        /// <summary>
        /// Event when a file is added to the monitoring folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            SaleCsvFile csvFile = SaleCsvFile.Create(e.FullPath);
            if (csvFile == null) { return; }
            if (!csvFile.CheckFileNameMatch(ConfigurationManager.AppSettings["FileNamePattern"]))
            {
                csvFile.Dispose();
                return;
            }
            NewFileDetectedEvent?.Invoke(this, csvFile.GetPath());
        }


        /// <summary>
        /// On FileWatcher Error event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _watcher_Error(object sender, ErrorEventArgs e)
        {
            string err = "!" + e.GetException().GetType().ToString();
            NewFileDetectedEvent?.Invoke(this, err);       
        }

        #endregion // FILESYSTEMWATCHER_EVENTS




        #region IDISPOSABLE
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



        #endregion // IDISPOSABLE


    }
}
