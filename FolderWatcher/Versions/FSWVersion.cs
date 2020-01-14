using System;
using System.IO;
using System.Threading;

namespace FolderWatcher.Versions
{
    public sealed class FSWVersion : FolderWatcherBase
    {
        private FileSystemWatcher _watcher = null;

        public FSWVersion(string path, string filter)
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
        public static FSWVersion CreateInstance(string path, string filter)
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

            return new FSWVersion(path, filter);
        }


        /// <summary>
        /// Launch watching 
        /// </summary>       
        public override void Launch()
        {
            if (this._watcher == null) { return; }

            this.IsLaunched = true;
            try
            {
                this._watcher.EnableRaisingEvents = true;
                while (IsLaunched) { Thread.Sleep(1000); }
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
                this.Stop();
            }
        }


        public override void Stop()
        {
            if (this._watcher == null) { return; }

            this._watcher.EnableRaisingEvents = false;
            IsLaunched = false;

        }




        #region IDISPOSABLE
        //##############################################################################################################     

        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
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

                this._disposed = true;
            }
        }



        #endregion // IDISPOSABLE





        #region FILESYSTEMWATCHER_EVENTS
        //####################################################################################################################

        /// <summary>
        /// Event when a file is added to the monitoring folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            if (IsFileReady(filePath))
            {
                OnNewFileDetected(filePath);
            }
        }

        /// <summary>
        /// On FileWatcher Error event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _watcher_Error(object sender, ErrorEventArgs e)
        {
            OnError(e.GetException().GetType().ToString());
            this.Stop();
                 
        }

        #endregion // FILESYSTEMWATCHER_EVENTS



    }
}
