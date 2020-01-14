using FDAL.BaseClasses;
using FDAL.Interfaces;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FDAL.Versions
{
    public sealed class SaleCsvFile : FileInfoBase, IRemovable
    {

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="path"></param>
        private SaleCsvFile(string path) : base(path)
        {
        }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SaleCsvFile Create(string path)
        {
            if (string.IsNullOrEmpty(path)) { return null; }

            SaleCsvFile scsvf = new SaleCsvFile(path);
            return scsvf.file == null ? null : scsvf;
        }


        /// <summary>
        /// Check file name match
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>true if fileName matches the pattern; else false</returns>
        public bool CheckFileNameMatch(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) { return false; }

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (regex.IsMatch(this.file.Name)) { return true; }
            return false;
        }


        /// <summary>
        /// Check file is ready to use
        /// </summary>
        /// <returns></returns>
        public bool CheckFileReadyToUse()
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(this.file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
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
        /// Delete file
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            if (this.file != null) 
            {
                try
                {
                    this.file.Delete();
                    this.Dispose();
                    return true;
                }
                catch (Exception )
                {
                }                
            }
            return false;
        }
    }
}
