using FolderWatcher.Interfaces;
using System;
using System.IO;
using System.Security.Permissions;

namespace FolderWatcher.Versions
{
    public abstract class FolderWatcherBase : ILauncable, IDisposable
    {
        public bool IsLaunched { get; protected set; } = false;
        protected bool _disposed = false;

        public event EventHandler<string> NewFileDetectedEvent;
        public event EventHandler<string> ErrordEvent;


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public abstract void Launch();

        public abstract void Stop();



        protected void OnNewFileDetected(string filePath)
        {
            NewFileDetectedEvent?.Invoke(this, filePath);
        }

        protected void OnError(string msg)
        {
            ErrordEvent?.Invoke(this, msg);
        }



        /// <summary>
        /// Chechk for file ready to use
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        protected bool IsFileReady(string filename)
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




        #region IDISPOSABLE
        //##############################################################################################################              


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Stop();
            Dispose(true);            
            GC.SuppressFinalize(this);  // говорим сборщику мусора, что наш объект уже освободил ресурсы 
        }

        protected abstract void Dispose(bool disposed);


        //// деструктор класса 
        //~FolderWatcher()
        //{
        //    Dispose(false);
        //}


        #endregion // IDISPOSABLE


    }
}
