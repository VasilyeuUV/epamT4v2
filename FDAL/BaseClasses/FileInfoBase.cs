using System;
using System.IO;

namespace FDAL.BaseClasses
{
    public abstract class FileInfoBase : IDisposable
    {
        protected FileInfo file = null;
                     
        protected FileInfoBase(string path)
        {
            try
            {
                file = new FileInfo(path);
                if (!file.Exists) { file = null; }
            }
            catch (Exception)
            {
                file = null;
            }
        }


        /// <summary>
        /// Chechk for file ready to use.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool CheckFileReadyToUse(string filename)
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


        /// <summary>
        /// Get file name
        /// </summary>
        /// <returns>empty string or file name</returns>
        public string GetName()
        {
            if (file == null) { return string.Empty; }
            return file.Name.ToLower();
        }

        /// <summary>
        /// Get file full name
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            if (file == null) { return string.Empty; }
            return file.FullName;
        }



        #region IDISPOSABLE
        //#############################################################################################################

        public virtual void Dispose()
        {
            if (file != null) { file = null; }         
        }

        #endregion // IDISPOSABLE


    }
}
