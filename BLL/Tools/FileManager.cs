using System;
using System.IO;
using System.Text.RegularExpressions;

namespace BLL.Tools
{
    internal static class FileManager
    {
        private static readonly object locker = new object();
        


        /// <summary>
        /// Check file availability and its name matching template 
        /// </summary>
        /// <param name="filePath">full path to file</param>
        /// <param name="pattern">file name Regexp pattern</param>
        /// <returns>null or FileInfo object</returns>
        internal static FileInfo GetFileToParse(string filePath, string pattern)
        {
            return CheckFileNameMatch(filePath, pattern) ? new FileInfo(filePath) : null;
        }



        /// <summary>
        /// Get file name from full path if file exist
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static string GetFileName(string filePath)
        {
            lock (locker)
            {
                FileInfo fileInf = GetFileInfo(filePath);
                return fileInf == null ? string.Empty : fileInf.Name;
            }
        }


        /// <summary>
        /// Get FileInfo object from path if file exist
        /// </summary>
        /// <param name="filePath">full file path </param>
        /// <returns>FileInfo object or null</returns>
        internal static FileInfo GetFileInfo(string filePath)
        {
            lock (locker)
            {
                FileInfo fileInf = new FileInfo(filePath);
                if (fileInf.Exists) { return fileInf; }
                return null;
            }
        }






        #region FILE_CHECK
        //##############################################################################################################
       
            /// <summary>
        /// Check file name match
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>true if fileName matches the pattern; else false</returns>
        internal static bool CheckFileNameMatch(string filePath, string pattern)
        {
            lock (locker)
            {
                if (string.IsNullOrWhiteSpace(pattern)) { return false; }

                string fileName = GetFileName(filePath);
                if (string.IsNullOrWhiteSpace(fileName)) { return false; }

                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(fileName)) { return true; }
                return false;
            }

        }

        /// <summary>
        /// Chechk for file ready to use.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        internal static bool CheckFileReadyToUse(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            lock (locker)
            {

                if (GetFileInfo(filename) == null) { return false; }
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
        }

        #endregion //FILE_CHECK



        #region FILE_OPERATIONS


        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="filePath"></param>
        internal static void DeleteFile(string filePath)
        {
            FileInfo fileInf = new FileInfo(filePath);
            if (fileInf.Exists) { fileInf.Delete(); }
        }


        #endregion //FILE_OPERATIONS


    }
}
