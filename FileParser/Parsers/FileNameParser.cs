using System;
using System.IO;

namespace FileParser.Parsers
{
    public sealed class FileNameParser
    {
        private static readonly object locker = new object();


        /// <summary>
        /// File name parser
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="pattern"></param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        public string[] GetFileNameParsingResult(string filePath, char[] delimiters = null)
        {
            lock (locker)
            {
                return ParseFileName(filePath, delimiters);
            }
        }



        private string[] ParseFileName(string filePath, char[] delimiters)
        {
            FileInfo csvFile = GetFileInfo(filePath);
            if (!csvFile.Exists) { return null; }

            if (delimiters == null || delimiters.Length < 1) { delimiters = new[] { '_' }; }

            return csvFile.Name.Split(delimiters);
        }




        private FileInfo GetFileInfo(string path)
        {
            FileInfo file = null;
            try
            {
                file = new FileInfo(path);
                if (!file.Exists) { file = null; }
            }
            catch (Exception)
            {
            }
            return file;
        }

    }
}
